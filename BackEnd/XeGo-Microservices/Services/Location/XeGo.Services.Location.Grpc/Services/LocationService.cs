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
                    var users = await dbContext.DriverLocations.AsNoTracking().Where(u => u.Geohash == geohash).Select(u => u.UserId).ToListAsync();
                    checkingUsersIdList.AddRange(users);
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
    }
}
