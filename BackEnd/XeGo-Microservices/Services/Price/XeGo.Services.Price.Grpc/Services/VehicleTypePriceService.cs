using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using XeGo.Services.Price.Grpc.Data;
using XeGo.Services.Price.Grpc.Protos;

namespace XeGo.Services.Price.Grpc.Services
{
    public class VehicleTypePriceService(AppDbContext db, ILogger<VehicleTypePriceService> logger) : VehicleTypePriceProtoService.VehicleTypePriceProtoServiceBase
    {
        private VehicleTypePriceResponse Response { get; set; } = new();

        public override async Task<VehicleTypePriceResponse> GetVehicleTypePriceById(GetVehicleTypePriceByIdRequest request, ServerCallContext context)
        {
            logger.LogInformation($"{nameof(VehicleTypePriceService)}>{nameof(GetVehicleTypePriceById)}: Begin.");
            try
            {
                var cVehicleTypePrice = await
                    db.VehicleTypePrices.FirstOrDefaultAsync(v => v.VehicleTypeId == request.VehicleTypeId);

                if (cVehicleTypePrice == null)
                {
                    Response.IsSuccess = false;
                    Response.Message = "Not found!";
                    logger.LogError($"{nameof(PriceService)}>{nameof(GetVehicleTypePriceById)}: Not Found!");
                    return Response;
                }

                Response.IsSuccess = true;
                Response.Data = JsonConvert.SerializeObject(cVehicleTypePrice);
                logger.LogInformation($"{nameof(PriceService)}>{nameof(GetVehicleTypePriceById)}: Completed.");
            }
            catch (Exception e)
            {
                Response.IsSuccess = false;
                Response.Message = e.Message;

                logger.LogError($"{nameof(PriceService)}>{nameof(GetVehicleTypePriceById)}: {e.Message}");
            }

            return Response;
        }
    }
}
