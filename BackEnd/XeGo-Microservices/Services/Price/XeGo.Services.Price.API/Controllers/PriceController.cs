using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using XeGo.Services.Price.API.Repository.IRepository;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Price.API.Controllers
{
    [Route("price")]
    [ApiController]
    public class PriceController(IPriceRepository priceRepo, ILogger<PriceController> logger) : ControllerBase
    {
        private ResponseDto ResponseDto = new();

        [HttpGet]
        public async Task<ResponseDto> GetAllPrice(
            int? rideId,
            int? discountId,
            int? vehicleTypeId,
            double? distanceInMetersStart,
            double? distanceInMetersEnd,
            double? pricePerKmStart,
            double? pricePerKmEnd,
            double? totalPriceStart,
            double? totalPriceEnd,
            int pageNumber = 0,
            int pageSize = 0)
        {
            logger.LogInformation($"Getting prices...");

            try
            {
                Expression<Func<Entities.Price, bool>> filters = p =>
                    (rideId == null || p.RideId == rideId) &&
                    (discountId == null || p.DiscountId == discountId) &&
                    (vehicleTypeId == null || p.VehicleTypeId == vehicleTypeId) &&
                    (distanceInMetersStart == null || p.DistanceInMeters >= distanceInMetersStart) &&
                    (distanceInMetersEnd == null || p.DistanceInMeters <= distanceInMetersEnd) &&
                    (pricePerKmStart == null || p.PricePerKm >= pricePerKmStart) &&
                    (pricePerKmEnd == null || p.PricePerKm <= pricePerKmEnd) &&
                    (totalPriceStart == null || p.TotalPrice >= totalPriceStart) &&
                    (totalPriceEnd == null || p.TotalPrice <= totalPriceEnd);

                var prices = await priceRepo.GetAllAsync(
                    filter: filters,
                    pageSize: pageSize,
                    pageNumber: pageNumber);

                logger.LogInformation($"Get prices : {prices.Count()} Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = prices;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(PriceController)}>{nameof(GetAllPrice)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> CreatePrice(CreatePriceRequestDto requestDto)
        {
            logger.LogInformation($"Executing {nameof(PriceController)}>{nameof(CreatePrice)} from userId:{requestDto.ModifiedBy}...");

            try
            {
                var createDto = new Entities.Price()
                {
                    RideId = requestDto.RideId,
                    DiscountId = requestDto.DiscountId,
                    VehicleTypeId = requestDto.VehicleTypeId,
                    DistanceInMeters = requestDto.DistanceInMeters,
                    PricePerKm = requestDto.PricePerKm,
                    CreatedBy = requestDto.ModifiedBy,
                    LastModifiedBy = requestDto.ModifiedBy,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow
                };
                createDto.CalculateTotalPrice();

                await priceRepo.CreateAsync(createDto);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = createDto;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(PriceController)}>{nameof(CreatePrice)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

    }
}
