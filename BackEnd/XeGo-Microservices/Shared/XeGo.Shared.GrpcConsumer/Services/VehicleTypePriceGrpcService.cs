using XeGo.Services.Price.Grpc.Protos;

namespace XeGo.Shared.GrpcConsumer.Services
{
    public class VehicleTypePriceGrpcService(VehicleTypePriceProtoService.VehicleTypePriceProtoServiceClient service)
    {
        private VehicleTypePriceProtoService.VehicleTypePriceProtoServiceClient _service = service ?? throw new ArgumentNullException(nameof(service));

        public async Task<VehicleTypePriceResponse> GetVehicleTypePriceById(int vehicleTypeId)
        {
            var request = new GetVehicleTypePriceByIdRequest()
            {
                VehicleTypeId = vehicleTypeId,
            };

            return await _service.GetVehicleTypePriceByIdAsync(request);
        }
    }
}
