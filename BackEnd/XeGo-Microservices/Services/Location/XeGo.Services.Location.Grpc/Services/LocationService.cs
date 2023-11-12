using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using XeGo.Services.Location.API.Services.IServices;
using XeGo.Services.Location.Grpc.Data;
using XeGo.Services.Location.Grpc.Protos;
using XeGo.Shared.GrpcConsumer.Services;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Location.Grpc.Services
{
    public class LocationService : LocationProtoService.LocationProtoServiceBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IGeoHashService _geoHashService;
        private readonly CodeValueGrpcService _codeValueGrpcService;
        private Response Response { get; set; }

        public LocationService(AppDbContext dbContext, IGeoHashService geoHashService, CodeValueGrpcService codeValueGrpcService)
        {
            _dbContext = dbContext;
            _geoHashService = geoHashService;
            _codeValueGrpcService = codeValueGrpcService;
            Response = new();
        }

        public override async Task<Response> FindNearbyDrivers(FindNearbyDriversRequest request,
            ServerCallContext context)
        {
            double geoSquareSideMeters = await GetGeoSquareSideMeters();
            double radiusInMeters = await GetRadiusInMeters();
            var neighborsGeohash = _geoHashService.GetNeighbors(request.Latitude, request.Longitude, geoSquareSideMeters, radiusInMeters * request.ExpandTimes);

            var usersList = new List<string>();
            foreach (var geohash in neighborsGeohash)
            {
                var users = await _dbContext.DriverLocations.AsNoTracking().Where(u => u.Geohash == geohash).Select(u => u.UserId).ToListAsync();
                usersList.AddRange(users);
            }

            Response.IsSuccess = true;
            Response.Data = JsonConvert.SerializeObject(usersList);
            return Response;
        }

        #region Private Methods

        private async Task<double> GetGeoSquareSideMeters()
        {
            var response = await _codeValueGrpcService
                .GetValue2(GeohashConstants.GeohashName, GeohashConstants.SquareSideLengthInMetersName, null, null);

            return Convert.ToDouble(response.Data);
        }

        private async Task<double> GetRadiusInMeters()
        {
            var response = await _codeValueGrpcService
                .GetValue2(GeohashConstants.GeohashName, GeohashConstants.RadiusInMetersName, null, null);

            return Convert.ToDouble(response.Data);
        }

        #endregion
    }
}
