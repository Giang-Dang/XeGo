using XeGo.Services.Vehicle.Grpc.Protos;

namespace XeGo.Shared.GrpcConsumer.Services
{
    public class VehicleGrpcService(VehicleProtoService.VehicleProtoServiceClient service)
    {
        public async Task<VehicleResponse> GetVehicleByDriverId(string driverId)
        {
            var request = new GetVehicleByDriverIdRequest { DriverId = driverId };
            return await service.GetVehicleByDriverIdAsync(request);
        }
    }
}
