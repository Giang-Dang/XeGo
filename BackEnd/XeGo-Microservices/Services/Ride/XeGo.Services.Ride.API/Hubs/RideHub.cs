using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using XeGo.Services.Ride.API.Data;
using XeGo.Services.Ride.API.Entities;
using XeGo.Services.Ride.API.Models.Dto;
using XeGo.Shared.GrpcConsumer.Services;
using XeGo.Shared.Lib.Constants;
using XeGo.Shared.Lib.Helpers;

namespace XeGo.Services.Ride.API.Hubs
{
    public class RideHub(
        ILogger<RideHub> logger,
        AppDbContext db,
        LocationGrpcService locationGrpcService) : Hub
    {
        private readonly ConcurrentDictionary<string, TaskCompletionSource<bool>> _pendingRides = new();
        public async Task<string> FindDriver(string fromUserId, int rideId)
        {
            try
            {
                var cRide = await GetRide(rideId);
                if (cRide == null)
                {
                    logger.LogError($"Ride not found! (id:{rideId})");
                    return "Not Found!";
                }

                var cRider = await GetUserConnectionAsync(fromUserId);

                var driverIdList = await GetNearbyDrivers(cRide);

                if (driverIdList != null)
                {
                    var acceptedDriverId = await AssignDriverToRide(driverIdList, cRide, cRider!);
                    if (acceptedDriverId == null)
                    {
                        logger.LogInformation($"Driver not found! (id:{rideId})");
                        return "Not Found!";
                    }

                    cRide.Status = RideStatusConstants.AwaitingPickup;
                    db.Rides.Update(cRide);
                    await db.SaveChangesAsync();

                    logger.LogInformation($"Driver found! (RideId:{rideId}; DriverId:{acceptedDriverId})");

                    return acceptedDriverId;
                }
                else
                {
                    logger.LogError($"Driver not found! (id:{rideId})");
                    return "Not Found!";
                }
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(FindDriver)}: {e.Message}");
                return "Not Found!";
            }
        }

        public void AcceptRide(string driverId, int rideId)
        {
            if (_pendingRides.TryRemove(driverId, out var tcs))
            {
                tcs.SetResult(true);
            }
        }

        public async Task UpdateLocation(string fromUserId, string toUserId, PositionDto newPosition)
        {
            try
            {
                var userConnectionId = await db.UserConnectionIds
                    .FirstOrDefaultAsync(u => u.UserId == toUserId);
                if (userConnectionId == null)
                {
                    logger.LogError("Destination Connection Id not found!");
                    return;
                }

                var toUserConnectionId = userConnectionId.ConnectionId;

                await Clients.Clients(toUserConnectionId).SendAsync("updateLocation", newPosition);
                logger.LogInformation($"{nameof(UpdateLocation)}: " +
                                      $"{fromUserId}'s location sent to {toUserId}");
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(UpdateLocation)}: {e.Message}");
            }

        }

        public async Task UpdateRideStatus(string fromUserId, string toUserId, int rideId, string newStatus)
        {
            try
            {
                var userConnectionId = await db.UserConnectionIds
                    .FirstOrDefaultAsync(u => u.UserId == toUserId);
                if (userConnectionId == null)
                {
                    logger.LogError($"Destination Connection Id not found! (userId: {toUserId})");
                    return;
                }

                var toUserConnectionId = userConnectionId.ConnectionId;

                var cRide = await db.Rides.FirstOrDefaultAsync(u => u.Id == rideId);
                if (cRide == null)
                {
                    logger.LogError($"Ride (id:{rideId}) not found!");
                    return;
                }
                cRide.Status = newStatus;
                await db.SaveChangesAsync();
                logger.LogInformation($"Ride status (id:{rideId}) has been updated to {newStatus}");

                await Clients.Clients(toUserConnectionId).SendAsync("updateRideStatus", newStatus);
                logger.LogInformation($"New ride status ({newStatus}-id:{rideId}) has " +
                                      $"been sent to {toUserConnectionId}");
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(UpdateRideStatus)}: {e.Message}");
            }

        }

        public async Task<bool> RegisterConnectionId(string userId)
        {
            logger.LogInformation($"{nameof(RideHub)} > {nameof(RegisterConnectionId)} : Triggered!");
            try
            {
                var connectionId = Context.ConnectionId;

                var cUserId = await db.UserConnectionIds.FirstOrDefaultAsync(u => u.UserId == userId);
                if (cUserId != null)
                {
                    // Update
                    cUserId.ConnectionId = connectionId;
                    await db.SaveChangesAsync();
                    logger.LogInformation($"{nameof(RegisterConnectionId)}: UserConnectionId has been updated! (userId: {userId})");
                }
                else
                {
                    // Create
                    var createDto = new UserConnectionId()
                    {
                        UserId = userId,
                        ConnectionId = connectionId,
                        CreatedBy = RoleConstants.System,
                        LastModifiedBy = RoleConstants.System,
                    };

                    await db.UserConnectionIds.AddAsync(createDto);
                    await db.SaveChangesAsync();
                    logger.LogInformation($"{nameof(RegisterConnectionId)}: UserConnectionId has been created! (userId: {userId})");
                }

                return true;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(RegisterConnectionId)}: {e.Message}");
                return false;
            }
        }

        public async Task RemoveConnectionId(string userId)
        {
            logger.LogInformation($"{nameof(RideHub)} > {nameof(RemoveConnectionId)} : Triggered!");
            try
            {
                var cUserConnectionId = await db.UserConnectionIds.FirstOrDefaultAsync(u => u.UserId == userId);
                if (cUserConnectionId == null)
                {
                    logger.LogError($"{nameof(RemoveConnectionId)}: Not Found!");
                    return;
                }

                db.UserConnectionIds.Remove(cUserConnectionId);
                await db.SaveChangesAsync();
                logger.LogInformation($"{nameof(RideHub)} > {nameof(RemoveConnectionId)} : Done!");
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(RemoveConnectionId)}: {e.Message}");
            }
        }

        public async Task SendLocation(double latitude, double longitude, string userId)
        {
            logger.LogInformation($"{nameof(RideHub)} > {nameof(SendLocation)} (userId:{userId}): Triggered!");

            try
            {
                var cUserConnectionId = await db.UserConnectionIds.FirstOrDefaultAsync(u => u.UserId == userId);

                if (cUserConnectionId == null)
                {
                    logger.LogError($"{nameof(SendLocation)} (userId:{userId}): Not Found!");
                    return;
                }

                await Clients.Clients(cUserConnectionId.ConnectionId)
                    .SendAsync("ReceiveLocation", latitude, longitude);
                logger.LogInformation($"{nameof(RideHub)} > {nameof(SendLocation)} (userId:{userId}): Done!");
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(SendLocation)} (userId:{userId}): {e.Message}");
            }

        }

        #region Private Methods

        private async Task<Entities.Ride?> GetRide(int rideId)
        {
            return await db.Rides.FirstOrDefaultAsync(r => r.Id == rideId);
        }

        private async Task<UserConnectionId?> GetUserConnectionAsync(string userId)
        {
            return await db.UserConnectionIds.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        private async Task<double> GetGeoHashSquareSideInMeters()
        {
            var cvGeoHashSquareSideInMeters = await db.CodeValues
                .FirstOrDefaultAsync(c =>
                    c.Name == GeohashConstants.GeohashName
                    && c.Value1 == GeohashConstants.GeoHashSquareSideInMeters);

            return CodeValueHelpers.GetOriginalValue(cvGeoHashSquareSideInMeters?.Value2 ?? "500",
                cvGeoHashSquareSideInMeters?.Value2Type ?? CodeValueTypeConstants.Double);
        }

        private async Task<double> GetMaxRadius()
        {
            var cvMaxRadius = await db.CodeValues
                .FirstOrDefaultAsync(c =>
                    c.Name == GeohashConstants.GeohashName
                    && c.Value1 == GeohashConstants.MaxRadiusInMetersName);

            return CodeValueHelpers.GetOriginalValue(cvMaxRadius?.Value2 ?? "500",
                cvMaxRadius?.Value2Type ?? CodeValueTypeConstants.Double);
        }

        private async Task<List<string>?> GetNearbyDrivers(Entities.Ride cRide)
        {
            var geoHashSquareSideInMeters = await GetGeoHashSquareSideInMeters();
            var maxRadius = await GetMaxRadius();

            var driverIdListJson = await locationGrpcService
                .FindNearbyDrivers(
                    latitude: cRide.StartLatitude,
                    longitude: cRide.StartLongitude,
                    geoHashSquareSideInMeters: geoHashSquareSideInMeters,
                    maxRadius: maxRadius
                );

            return JsonConvert.DeserializeObject<List<string>>(driverIdListJson.Data);
        }

        private async Task<List<UserConnectionId>> GetDriverList(List<string>? driverIdList)
        {
            if (driverIdList == null)
            {
                return new List<UserConnectionId>();
            }

            return await db.UserConnectionIds
                .Where(u => driverIdList.Contains(u.UserId))
                .ToListAsync();
        }

        private async Task<bool> SendRideRequestAndWaitForResponse(UserConnectionId driver)
        {
            var tcs = new TaskCompletionSource<bool>();
            _pendingRides.TryAdd(driver.UserId, tcs);

            await Clients.Clients(driver.ConnectionId).SendAsync("receiveRide");

            // Wait for the driver to accept the ride or for 30 seconds to pass, whichever comes first
            var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(30)));

            // If the driver accepted the ride within 30 seconds, return true
            return completedTask == tcs.Task && await tcs.Task;
        }

        private void LogError(string message)
        {
            logger.LogError(message);
        }

        private async Task InformRiderNoDriverFound(UserConnectionId cRider)
        {
            await Clients.Clients(cRider.UserId).SendAsync("cannotFindDriver");
        }

        private async Task<string?> AssignDriverToRide(List<string> driverIdList, Entities.Ride cRide, UserConnectionId cRider)
        {
            var driverList = await GetDriverList(driverIdList);

            foreach (var driver in driverList)
            {
                if (await SendRideRequestAndWaitForResponse(driver))
                {
                    cRide.DriverId = driver.UserId;
                    await db.SaveChangesAsync();

                    return driver.UserId;
                }
            }

            return null;
        }

        #endregion
    }
}
