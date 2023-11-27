using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using XeGo.Services.Price.API.Entities;
using XeGo.Services.Price.API.Models;
using XeGo.Services.Price.API.Repository.IRepository;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Price.API.Controllers
{
    [Route("api/vehicle-type-price")]
    [ApiController]
    public class VehicleTypePriceController(IVehicleTypePriceRepository vehicleTypePriceRepo, ILogger<VehicleTypePrice> logger)
        : ControllerBase
    {
        private ResponseDto ResponseDto { get; set; } = new();

        [HttpGet]
        public async Task<ResponseDto> GetAllVehicleTypePrice(
            int? vehicleTypeId,
            double? pricePerKmStart,
            double? pricePerKmEnd,
            double? dropChargeStart,
            double? dropChargeEnd,
            int pageNumber = 1,
            int pageSize = 0)
        {
            logger.LogInformation($"Executing {nameof(VehicleTypePriceController)}>{nameof(GetAllVehicleTypePrice)}...");

            try
            {
                Expression<Func<Entities.VehicleTypePrice, bool>> filters = v =>
                    (vehicleTypeId == null || v.VehicleTypeId == vehicleTypeId) &&
                    (pricePerKmStart == null || v.PricePerKm >= pricePerKmStart) &&
                    (pricePerKmEnd == null || v.PricePerKm <= pricePerKmEnd) &&
                    (dropChargeStart == null || v.DropCharge >= dropChargeStart) &&
                    (dropChargeEnd == null || v.DropCharge <= dropChargeEnd);

                var vehicleTypePrices = await vehicleTypePriceRepo.GetAllAsync(
                    filter: filters,
                    pageSize: pageSize,
                    pageNumber: pageNumber);

                logger.LogInformation($"Get vehicle type prices : {vehicleTypePrices.Count()} Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = vehicleTypePrices;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(VehicleTypePriceController)}>{nameof(GetAllVehicleTypePrice)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> CreateVehicleTypePrice(CreateVehicleTypePriceRequestDto requestDto)
        {
            logger.LogInformation($"Executing {nameof(VehicleTypePriceController)}>{nameof(CreateVehicleTypePrice)} from userId:{requestDto.ModifiedBy}...");

            try
            {
                var createDto = new VehicleTypePrice()
                {
                    VehicleTypeId = requestDto.VehicleTypeId,
                    PricePerKm = requestDto.PricePerKm,
                    DropCharge = requestDto.DropCharge,
                    CreatedBy = requestDto.ModifiedBy,
                    LastModifiedBy = requestDto.ModifiedBy,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow
                };

                await vehicleTypePriceRepo.CreateAsync(createDto);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = createDto;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(VehicleTypePriceController)}>{nameof(CreateVehicleTypePrice)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPut]
        public async Task<ResponseDto> EditVehicleTypePrice(EditVehicleTypePriceRequestDto requestDto)
        {
            logger.LogInformation($"Executing {nameof(VehicleTypePriceController)}>{nameof(EditVehicleTypePrice)} from userId:{requestDto.ModifiedBy}...");

            try
            {
                var cEntity = await vehicleTypePriceRepo.GetAsync(v => v.VehicleTypeId == requestDto.VehicleTypeId);
                if (cEntity == null)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = "Not found!";
                    return ResponseDto;
                }

                cEntity.PricePerKm = requestDto.PricePerKm ?? cEntity.PricePerKm;
                cEntity.DropCharge = requestDto.DropCharge ?? cEntity.DropCharge;
                cEntity.LastModifiedBy = requestDto.ModifiedBy;
                cEntity.LastModifiedDate = DateTime.UtcNow;

                await vehicleTypePriceRepo.UpdateAsync(cEntity);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = cEntity;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(VehicleTypePriceController)}>{nameof(EditVehicleTypePrice)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }
    }
}
