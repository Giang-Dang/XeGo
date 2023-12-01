using XeGo.Services.Price.Grpc.Protos;

namespace XeGo.Shared.GrpcConsumer.Services
{
    public class PriceGrpcService(PriceProtoService.PriceProtoServiceClient service)
    {
        private readonly PriceProtoService.PriceProtoServiceClient _service = service ?? throw new ArgumentNullException(nameof(service));

        public async Task<PriceResponse> GetPriceByRideId(int rideId)
        {
            var request = new GetPriceByRideIdRequest()
            {
                RideId = rideId,
            };

            return await _service.GetPriceByRideIdAsync(request);
        }

        public async Task<PriceResponse> CreatePrice(
            int rideId,
            int? discountId,
            int vehicleTypeId,
            double distanceInMeters,
            string modifiedBy)
        {
            var request = new CreatePriceRequest()
            {
                RideId = rideId,
                DiscountId = discountId,
                VehicleTypeId = vehicleTypeId,
                DistanceInMeters = distanceInMeters,
                ModifiedBy = modifiedBy,
            };

            return await _service.CreatePriceAsync(request);
        }

        public async Task<PriceResponse> EditPrice(
            int rideId,
            int? discountId,
            int? vehicleTypeId,
            int? distanceInMeters,
            string modifiedBy)
        {
            var request = new EditPriceRequest()
            {
                RideId = rideId,
                DiscountId = discountId,
                VehicleTypeId = vehicleTypeId,
                DistanceInMeters = distanceInMeters,
                ModifiedBy = modifiedBy,
            };

            return await _service.EditPriceAsync(request);
        }
    }
}
