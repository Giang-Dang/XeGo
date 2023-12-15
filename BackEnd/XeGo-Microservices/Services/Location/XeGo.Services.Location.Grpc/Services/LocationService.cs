using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using XeGo.Services.Location.API.Services.IServices;
using XeGo.Services.Location.Grpc.Data;
using XeGo.Services.Location.Grpc.Entities;
using XeGo.Services.Location.Grpc.Protos;
using XeGo.Shared.GrpcConsumer.Services;

namespace XeGo.Services.Location.Grpc.Services
{
    public class LocationService(
        AppDbContext dbContext,
        IGeoHashService geoHashService,
        VehicleGrpcService vehicleService,
        ILogger<LocationService> logger) : LocationProtoService.LocationProtoServiceBase
    {
        private Response Response { get; set; } = new();

        public override async Task<Response> FindNearbyDrivers(FindNearbyDriversRequest request,
            ServerCallContext context)
        {
            logger.LogInformation($"{nameof(LocationService)}>{nameof(FindNearbyDrivers)}: Begin.");
            logger.LogInformation($"{nameof(LocationService)}>{nameof(FindNearbyDrivers)} request: {JsonConvert.SerializeObject(request)}");

            try
            {
                var neighborsGeohash = geoHashService.GetNeighbors(request.Latitude, request.Longitude, request.GeoHashSquareSideInMeters, request.MaxRadius);

                var checkingUsersIdList = new List<string>();
                foreach (var geohash in neighborsGeohash)
                {
                    var drivers = await dbContext.DriverLocations.AsNoTracking().Where(u => u.Geohash == geohash).ToListAsync();
                    SortDriversByDistance(drivers, request.Latitude, request.Longitude);
                    var driverIds = drivers.Select(d => d.UserId).ToList();
                    checkingUsersIdList.AddRange(driverIds);
                }

                var returnedUsersIdList = new List<string>();

                foreach (var userId in checkingUsersIdList)
                {
                    var vehicle = await vehicleService.GetVehicleByDriverId(userId);
                    logger.LogInformation($"{nameof(LocationService)}>{nameof(FindNearbyDrivers)} vehicle: {JsonConvert.SerializeObject(vehicle)} (userId: {userId})");
                    if (vehicle == null || vehicle.Data == null)
                    {
                        continue;
                    }

                    var vehicleEntity = JsonConvert.DeserializeObject<VehicleDto>(vehicle.Data);
                    if (vehicleEntity == null)
                    {
                        logger.LogError($"{nameof(LocationService)}>{nameof(FindNearbyDrivers)}: Cannot DeserializeObject VehicleDto! {userId}");
                        continue;
                    }

                    if (vehicleEntity.TypeId == request.VehicleTypeId)
                    {
                        logger.LogInformation($"{nameof(LocationService)}>{nameof(FindNearbyDrivers)}: added userId: vehicleEntity {JsonConvert.SerializeObject(vehicleEntity)}");
                        logger.LogInformation($"{nameof(LocationService)}>{nameof(FindNearbyDrivers)}: added userId: {userId}");
                        returnedUsersIdList.Add(userId);
                    }
                }

                Response.IsSuccess = true;
                Response.Data = JsonConvert.SerializeObject(returnedUsersIdList);

                logger.LogInformation($"{nameof(LocationService)}>{nameof(FindNearbyDrivers)}: Completed.");

                return Response;
            }
            catch (Exception e)
            {
                Response.IsSuccess = false;
                Response.Message = e.Message;

                logger.LogError($"{nameof(LocationService)}>{nameof(FindNearbyDrivers)}: {e.Message}");

                return Response;
            }

        }

        #region Private Methods
        public double CalculateDistance(double startLat, double startLng, double endLat, double endLng)
        {
            var dLat = (endLat - startLat) * Math.PI / 180.0;
            var dLng = (endLng - startLng) * Math.PI / 180.0;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(startLat * Math.PI / 180.0) * Math.Cos(endLat * Math.PI / 180.0) *
                    Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            const double radius = 6371;
            return radius * c;
        }
        public void SortDriversByDistance(List<DriverLocation> drivers, double riderLat, double riderLng)
        {
            drivers.Sort((driver1, driver2) =>
            {
                var distanceToDriver1 = CalculateDistance(riderLat, riderLng, driver1.Latitude, driver1.Longitude);
                var distanceToDriver2 = CalculateDistance(riderLat, riderLng, driver2.Latitude, driver2.Longitude);
                return distanceToDriver1.CompareTo(distanceToDriver2);
            });
        }

        #endregion
    }
}
