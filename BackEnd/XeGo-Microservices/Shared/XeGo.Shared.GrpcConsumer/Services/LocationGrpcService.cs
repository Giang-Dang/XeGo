using XeGo.Services.Location.Grpc.Protos;

namespace XeGo.Shared.GrpcConsumer.Services
{
    public class LocationGrpcService(LocationProtoService.LocationProtoServiceClient service)
    {
        private LocationProtoService.LocationProtoServiceClient _service = service ?? throw new ArgumentNullException(nameof(service));

        public async Task<Response> FindNearbyDrivers(double latitude, double longitude, double geoHashSquareSideInMeters, double maxRadius, int vehicleTypeId)
        {
            var request = new FindNearbyDriversRequest()
            {
                Latitude = latitude,
                Longitude = longitude,
                GeoHashSquareSideInMeters = geoHashSquareSideInMeters,
                MaxRadius = maxRadius,
                VehicleTypeId = vehicleTypeId,
            };

            return await _service.FindNearbyDriversAsync(request);
        }
    }
}
