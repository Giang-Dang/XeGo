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
        private int secondsToCancelDriverRequest = 60;
        private static Dictionary<int, string?> rideDrivers = new();
        private static Dictionary<int, CancellationTokenSource> rideCts = new();

        public async Task<string?> FindDriver(string fromUserId, Entities.Ride ride, double totalPrice, string directionResponseDtoJson)
        {
            logger.LogInformation($"{nameof(RideHub)}>{nameof(FindDriver)}: Triggered!");
            try
            {
                logger.LogInformation($"{nameof(RideHub)}>{nameof(FindDriver)} rideJson : {JsonConvert.SerializeObject(ride)}");
                logger.LogInformation($"{nameof(RideHub)}>{nameof(FindDriver)} directionReponseDtoJson : {directionResponseDtoJson}");

                rideDrivers.Add(ride.Id, null);

                var cRider = await GetUserConnectionAsync(fromUserId);

                var driverIdList = await GetNearbyDrivers(ride);

                if (driverIdList == null)
                {
                    logger.LogError($"Driver not found! (id:{ride.Id})");
                    return null;
                }

                var rideJson = JsonConvert.SerializeObject(ride);
                var driverConnectionsList = await GetDriverList(driverIdList);

                foreach (var driver in driverConnectionsList)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.Token.ThrowIfCancellationRequested();
                    rideCts[ride.Id] = cts;

                    await Clients.Clients(driver.ConnectionId).SendAsync("receiveRide", rideJson, totalPrice, directionResponseDtoJson);

                    logger.LogInformation($"{nameof(RideHub)}>{nameof(FindDriver)}: sent receiveRide to {driver.UserId}, connectionId: {driver.ConnectionId}");

                    Task countDownTask = Task.Delay(TimeSpan.FromSeconds(secondsToCancelDriverRequest), cts.Token);

                    try
                    {
                        await countDownTask;
                        logger.LogInformation($"{nameof(RideHub)}>{nameof(FindDriver)}: countDownTask started!");
                    }
                    catch (Exception ex)
                    {
                        logger.LogInformation($"{nameof(RideHub)}>{nameof(FindDriver)}: countDownTask cancelled {ex}");
                    }

                    if (rideDrivers[ride.Id] != null)
                    {
                        logger.LogInformation($"{nameof(RideHub)}>{nameof(FindDriver)}: Done! (Drivers Found! - id: {rideDrivers[ride.Id]})");
                        return rideDrivers[ride.Id];
                    }
                }

                logger.LogInformation($"{nameof(RideHub)}>{nameof(FindDriver)}: Done! (Drivers not found!)");
                return null;
            }
            catch (AggregateException ae)
            {
                if (ae.InnerExceptions.Any(e => e is OperationCanceledException))
                {
                    if (rideDrivers[ride.Id] != null)
                    {
                        logger.LogInformation($"{nameof(RideHub)}>{nameof(FindDriver)}: Done! (Drivers Found! - id: {rideDrivers[ride.Id]})");
                        return rideDrivers[ride.Id];
                    }
                }

                logger.LogError($"{nameof(FindDriver)} ae: {ae.Message}");
                return null;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(FindDriver)} e: {e.Message}");
                return null;
            }
        }

        public bool AcceptRide(string driverId, int rideId)
        {
            logger.LogInformation($"{nameof(RideHub)}>{nameof(AcceptRide)}: Triggered!");

            rideDrivers[rideId] = driverId;

            var cts = rideCts[rideId];
            if (cts == null)
            {
                logger.LogError($"{nameof(RideHub)}>{nameof(AcceptRide)}: cts null! (false)");
                return false;
            }

            cts.Cancel();

            logger.LogInformation($"{nameof(RideHub)}>{nameof(AcceptRide)}: true {rideDrivers[rideId]}");

            return true;
        }

        public bool DeclineRide(string driverId, int rideId)
        {
            logger.LogInformation($"{nameof(RideHub)}>{nameof(DeclineRide)}: Triggered!");

            logger.LogInformation($"{nameof(RideHub)}>{nameof(DeclineRide)} driverId: {driverId}");
            logger.LogInformation($"{nameof(RideHub)}>{nameof(DeclineRide)} rideId: {rideId}");

            var cts = rideCts[rideId];
            if (cts == null)
            {
                logger.LogError($"{nameof(RideHub)}>{nameof(DeclineRide)}: cts null! (false)");
                return false;
            }

            cts.Cancel();
            logger.LogError($"{nameof(RideHub)}>{nameof(DeclineRide)}: true");
            return true;
        }

        public async Task UpdateLocation(string fromUserId, string toUserId, PositionDto newPosition)
        {
            logger.LogInformation($"{nameof(RideHub)}>{nameof(UpdateLocation)}: Triggered!");

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
            logger.LogInformation($"{nameof(RideHub)}>{nameof(UpdateRideStatus)}: Triggered!");

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

        public async Task SendLocation(string toUserId, double latitude, double longitude)
        {
            logger.LogInformation($"{nameof(RideHub)} > {nameof(SendLocation)} (to userId:{toUserId}): Triggered!");

            try
            {
                var cUserConnectionId = await db.UserConnectionIds.FirstOrDefaultAsync(u => u.UserId == toUserId);

                if (cUserConnectionId == null)
                {
                    logger.LogError($"{nameof(SendLocation)} (to userId:{toUserId}): Not Found!");
                    return;
                }

                await Clients.Clients(cUserConnectionId.ConnectionId)
                    .SendAsync("ReceiveLocation", latitude, longitude);
                logger.LogInformation($"{nameof(RideHub)} > {nameof(SendLocation)} (to userId:{toUserId}): Done!");
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(SendLocation)} (to userId:{toUserId}): {e.Message}");
            }

        }

        #region Private Methods

        private async Task<Entities.Ride?> GetRide(int rideId)
        {
            logger.LogInformation($"{nameof(RideHub)} > {nameof(GetRide)}: Triggered!");

            return await db.Rides.FirstOrDefaultAsync(r => r.Id == rideId);
        }

        private async Task<UserConnectionId?> GetUserConnectionAsync(string userId)
        {
            logger.LogInformation($"{nameof(RideHub)} > {nameof(GetUserConnectionAsync)}: Triggered!");

            return await db.UserConnectionIds.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        private async Task<double> GetGeoHashSquareSideInMeters()
        {
            logger.LogInformation($"{nameof(RideHub)} > {nameof(GetGeoHashSquareSideInMeters)}: Triggered!");

            var cvGeoHashSquareSideInMeters = await db.CodeValues
                .FirstOrDefaultAsync(c =>
                    c.Name == GeohashConstants.GeohashName
                    && c.Value1 == GeohashConstants.GeoHashSquareSideInMeters);

            return CodeValueHelpers.GetOriginalValue(cvGeoHashSquareSideInMeters?.Value2 ?? "500",
                cvGeoHashSquareSideInMeters?.Value2Type ?? CodeValueTypeConstants.Double);
        }

        private async Task<double> GetMaxRadius()
        {
            logger.LogInformation($"{nameof(RideHub)} > {nameof(GetMaxRadius)}: Triggered!");

            var cvMaxRadius = await db.CodeValues
                .FirstOrDefaultAsync(c =>
                    c.Name == GeohashConstants.GeohashName
                    && c.Value1 == GeohashConstants.MaxRadiusInMetersName);

            return CodeValueHelpers.GetOriginalValue(cvMaxRadius?.Value2 ?? "500",
                cvMaxRadius?.Value2Type ?? CodeValueTypeConstants.Double);
        }

        private async Task<List<string>?> GetNearbyDrivers(Entities.Ride cRide)
        {
            logger.LogInformation($"{nameof(RideHub)} > {nameof(GetNearbyDrivers)}: Triggered!");

            var geoHashSquareSideInMeters = await GetGeoHashSquareSideInMeters();
            var maxRadius = await GetMaxRadius();

            var driverIdListJson = await locationGrpcService
                .FindNearbyDrivers(
                    latitude: cRide.StartLatitude,
                    longitude: cRide.StartLongitude,
                    geoHashSquareSideInMeters: geoHashSquareSideInMeters,
                    maxRadius: maxRadius
                );

            var driverIdList = JsonConvert.DeserializeObject<List<string>>(driverIdListJson.Data);

            logger.LogInformation($"{nameof(RideHub)} > {nameof(GetNearbyDrivers)} > DriverIdList: {string.Join(',', driverIdList)}");

            return driverIdList;
        }

        private async Task<List<UserConnectionId>> GetDriverList(List<string>? driverIdList)
        {
            logger.LogInformation($"{nameof(RideHub)} > {nameof(GetDriverList)}: Triggered!");

            if (driverIdList == null)
            {
                return new List<UserConnectionId>();
            }

            return await db.UserConnectionIds
                .Where(u => driverIdList.Contains(u.UserId))
                .ToListAsync();
        }

        #endregion
    }
}
