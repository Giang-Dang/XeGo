using Microsoft.AspNetCore.Mvc;
using XeGo.Services.Vehicle.API.Entities;
using XeGo.Services.Vehicle.API.Models;
using XeGo.Services.Vehicle.API.Repository.IRepository;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Vehicle.API.Controllers
{
    [Route("api/vehicles/types")]
    [ApiController]
    public class VehicleTypeController(
            IVehicleTypeRepository vehicleTypeRepo,
            ILogger<VehicleTypeController> logger)
        : ControllerBase
    {
        private ResponseDto ResponseDto { get; set; } = new();

        [HttpGet]
        public async Task<ResponseDto> GetAllVehicleTypes()
        {
            logger.LogInformation($"Executing {nameof(VehicleTypeController)}>{nameof(GetAllVehicleTypes)} ...");

            try
            {
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = await vehicleTypeRepo.GetAllAsync();
                logger.LogInformation($"Executing {nameof(VehicleTypeController)}>{nameof(GetAllVehicleTypes)} completed!");
            }
            catch (Exception e)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = $"{e.Message}";
                logger.LogError($"{nameof(VehicleTypeController)}>{nameof(GetAllVehicleTypes)}: {e.Message}");
            }

            return ResponseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> CreateVehicleType(CreateVehicleTypeRequestDto requestDto)
        {
            logger.LogInformation($"Executing {nameof(VehicleTypeController)}>{nameof(CreateVehicleType)} ...");

            try
            {
                var typeExists = await vehicleTypeRepo.AnyAsync(t => t.Name == requestDto.Name);
                if (typeExists)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = "This Vehicle Type is already existed";
                    return ResponseDto;
                }

                var createEntity = new VehicleType()
                {
                    Name = requestDto.Name,
                    CreatedBy = requestDto.ModifiedBy,
                    LastModifiedBy = requestDto.ModifiedBy,
                    IsActive = requestDto.IsActive,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow
                };

                ResponseDto.Data = await vehicleTypeRepo.CreateAsync(createEntity);
                ResponseDto.IsSuccess = true;
                logger.LogInformation($"Executing {nameof(VehicleTypeController)}>{nameof(CreateVehicleType)} completed!");

            }
            catch (Exception e)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = $"{e.Message}";
                logger.LogError($"{nameof(VehicleTypeController)}>{nameof(CreateVehicleType)}: {e.Message}");
            }

            return ResponseDto;
        }

        [HttpPut]
        public async Task<ResponseDto> EditVehicleType(EditVehicleTypeRequestDto requestDto)
        {
            logger.LogInformation($"Executing {nameof(VehicleTypeController)}>{nameof(EditVehicleType)} ...");

            try
            {
                var cVehicleTypes = await vehicleTypeRepo.GetAsync(t => t.Id == requestDto.Id);
                if (cVehicleTypes == null)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = "This Vehicle Type is not found!";
                    return ResponseDto;
                }

                cVehicleTypes.Name = requestDto.Name ?? cVehicleTypes.Name;
                cVehicleTypes.IsActive = requestDto.IsActive ?? cVehicleTypes.IsActive;
                cVehicleTypes.LastModifiedBy = requestDto.ModifiedBy;
                cVehicleTypes.LastModifiedDate = DateTime.UtcNow;

                ResponseDto.Data = await vehicleTypeRepo.UpdateAsync(cVehicleTypes);
                ResponseDto.IsSuccess = true;
                logger.LogInformation($"Executing {nameof(VehicleTypeController)}>{nameof(EditVehicleType)} completed!");
            }
            catch (Exception e)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = $"{e.Message}";
                logger.LogError($"{nameof(VehicleTypeController)}>{nameof(EditVehicleType)}: {e.Message}");
            }

            return ResponseDto;
        }
    }
}
