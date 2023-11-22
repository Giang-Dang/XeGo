using XeGo.Services.Location.Grpc.Protos;

namespace XeGo.Shared.GrpcConsumer.Services
{
    public class LocationGrpcService
    {
        private LocationProtoService.LocationProtoServiceClient _service;

        public LocationGrpcService(LocationProtoService.LocationProtoServiceClient service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public async Task<Response> FindNearbyDrivers(double latitude, double longitude, double geoHashSquareSideInMeters, double maxRadius)
        {
            var request = new FindNearbyDriversRequest()
            {
                Latitude = latitude,
                Longitude = longitude,
                GeoHashSquareSideInMeters = geoHashSquareSideInMeters,
                MaxRadius = maxRadius
            };

            return await _service.FindNearbyDriversAsync(request);
        }
    }
}
