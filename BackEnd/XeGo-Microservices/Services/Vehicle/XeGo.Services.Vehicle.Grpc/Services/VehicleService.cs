using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using XeGo.Services.Vehicle.Grpc.Data;
using XeGo.Services.Vehicle.Grpc.Protos;

namespace XeGo.Services.Vehicle.Grpc.Services
{
    public class VehicleService(
        AppDbContext db,
        ILogger<VehicleService> logger
        ) : VehicleProtoService.VehicleProtoServiceBase
    {
        private VehicleResponse Response { get; set; } = new();

        public override async Task<VehicleResponse> GetVehicleByDriverId(GetVehicleByDriverIdRequest request, ServerCallContext context)
        {
            logger.LogInformation($"{nameof(VehicleService)} > {nameof(GetVehicleByDriverId)}: Triggered!");

            try
            {
                var driverVehicleQuery =
                    from dv in db.DriverVehicles
                    join v in db.Vehicles on dv.VehicleId equals v.Id
                    where dv.DriverId == request.DriverId
                    select v;

                var driverVehicle = await driverVehicleQuery.FirstOrDefaultAsync();

                Response.IsSuccess = true;
                Response.Data = JsonConvert.SerializeObject(driverVehicle);
            }
            catch (System.Exception ex)
            {
                Response.IsSuccess = false;
                Response.Message = ex.Message;
                logger.LogError($"{nameof(VehicleService)}>{nameof(GetVehicleByDriverId)}: {ex.Message}");
                throw;
            }

            return Response;
        }
    }
}
