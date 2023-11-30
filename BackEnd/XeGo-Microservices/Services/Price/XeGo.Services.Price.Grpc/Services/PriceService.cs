using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using XeGo.Services.Price.Grpc.Data;
using XeGo.Services.Price.Grpc.Protos;

namespace XeGo.Services.Price.Grpc.Services
{
    public class PriceService(AppDbContext db, ILogger<PriceService> logger) : PriceProtoService.PriceProtoServiceBase
    {
        private PriceResponse Response { get; set; } = new();

        public override async Task<PriceResponse> GetPriceByRideId(GetPriceByRideIdRequest request, ServerCallContext context)
        {
            logger.LogInformation($"{nameof(PriceService)}>{nameof(GetPriceByRideId)}: Begin.");

            try
            {
                var cPrice = await db.Prices.FirstOrDefaultAsync(p => p.RideId == request.RideId);
                if (cPrice == null)
                {
                    Response.IsSuccess = false;
                    Response.Message = "Not found!";
                    return Response;
                }


                Response.IsSuccess = true;
                Response.Data = JsonConvert.SerializeObject(cPrice);

                logger.LogInformation($"{nameof(PriceService)}>{nameof(GetPriceByRideId)}: Completed.");

                return Response;
            }
            catch (Exception e)
            {
                Response.IsSuccess = false;
                Response.Message = e.Message;

                logger.LogError($"{nameof(PriceService)}>{nameof(GetPriceByRideId)}: {e.Message}");

                return Response;
            }

        }

        public override async Task<PriceResponse> CreatePrice(CreatePriceRequest request, ServerCallContext context)
        {
            logger.LogInformation($"{nameof(PriceService)}>{nameof(CreatePrice)}: Begin.");

            try
            {
                var cPrice = await db.Prices.FirstOrDefaultAsync(p => p.RideId == request.RideId);
                if (cPrice != null)
                {
                    logger.LogError($"{nameof(PriceService)}>{nameof(CreatePrice)}: Already exists!");
                    Response.IsSuccess = false;
                    Response.Message = "Already exists!";
                    return Response;
                }

                var createDto = new Entities.Price()
                {
                    RideId = request.RideId,
                    DiscountId = request.DiscountId,
                    VehicleTypeId = request.VehicleTypeId,
                    DistanceInMeters = request.DistanceInMeters,
                    CreatedBy = request.ModifiedBy,
                    LastModifiedBy = request.ModifiedBy,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow
                };
                createDto.CalculateTotalPrice();

                await db.Prices.AddAsync(createDto);
                await db.SaveChangesAsync();
                Response.IsSuccess = true;
                Response.Data = JsonConvert.SerializeObject(createDto);

                logger.LogInformation($"{nameof(PriceService)}>{nameof(CreatePrice)}: Completed.");
            }
            catch (Exception e)
            {
                Response.IsSuccess = false;
                Response.Message = e.Message; ;
                logger.LogError($"{nameof(PriceService)}>{nameof(CreatePrice)}: {e.Message}");
            }

            return Response;
        }

        public override async Task<PriceResponse> EditPrice(EditPriceRequest request, ServerCallContext context)
        {
            logger.LogInformation($"{nameof(PriceService)}>{nameof(EditPrice)}: Begin.");

            try
            {
                var cPrice = await db.Prices.FirstOrDefaultAsync(p => p.RideId == request.RideId);
                if (cPrice == null)
                {
                    Response.IsSuccess = false;
                    Response.Message = "Not found!";
                    logger.LogError($"{nameof(PriceService)}>{nameof(EditPrice)}: Not found!");
                    return Response;
                }

                cPrice.DiscountId = request.DiscountId ?? cPrice.DiscountId;
                cPrice.VehicleTypeId = request.VehicleTypeId ?? cPrice.VehicleTypeId;
                cPrice.DistanceInMeters = request.DistanceInMeters ?? cPrice.DistanceInMeters;
                cPrice.LastModifiedBy = request.ModifiedBy;
                cPrice.LastModifiedDate = DateTime.UtcNow;
                cPrice.CalculateTotalPrice();

                db.Prices.Update(cPrice);
                await db.SaveChangesAsync();

                Response.IsSuccess = true;
                Response.Data = JsonConvert.SerializeObject(cPrice);
                logger.LogInformation($"{nameof(PriceService)}>{nameof(EditPrice)}: Completed.");
            }
            catch (Exception e)
            {
                Response.IsSuccess = false;
                Response.Message = e.Message; ;
                logger.LogError($"{nameof(PriceService)}>{nameof(EditPrice)}: {e.Message}");
            }

            return Response;
        }
    }
}
