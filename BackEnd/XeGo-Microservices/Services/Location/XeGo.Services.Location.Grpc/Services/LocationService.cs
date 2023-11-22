using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using XeGo.Services.Location.API.Services.IServices;
using XeGo.Services.Location.Grpc.Data;
using XeGo.Services.Location.Grpc.Protos;

namespace XeGo.Services.Location.Grpc.Services
{
    public class LocationService(AppDbContext dbContext, IGeoHashService geoHashService) : LocationProtoService.LocationProtoServiceBase
    {
        private Response Response { get; set; } = new();

        public override async Task<Response> FindNearbyDrivers(FindNearbyDriversRequest request,
            ServerCallContext context)
        {
            var neighborsGeohash = geoHashService.GetNeighbors(request.Latitude, request.Longitude, request.GeoHashSquareSideInMeters, request.MaxRadius);

            var usersIdList = new List<string>();
            foreach (var geohash in neighborsGeohash)
            {
                var users = await dbContext.DriverLocations.AsNoTracking().Where(u => u.Geohash == geohash).Select(u => u.UserId).ToListAsync();
                usersIdList.AddRange(users);
            }

            Response.IsSuccess = true;
            Response.Data = JsonConvert.SerializeObject(usersIdList);
            return Response;
        }
    }
}
