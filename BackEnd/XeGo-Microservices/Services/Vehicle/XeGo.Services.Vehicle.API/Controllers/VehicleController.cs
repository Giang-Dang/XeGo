using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using XeGo.Services.Vehicle.API.Entities;
using XeGo.Services.Vehicle.API.Models;
using XeGo.Services.Vehicle.API.Repository.IRepository;
using XeGo.Shared.Lib.Helpers;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Vehicle.API.Controllers
{
    [Route("api/vehicles")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepo;
        private readonly IVehicleBanRepository _vehicleBanRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleController> _logger;
        private ResponseDto ResponseDto { get; set; }
        public VehicleController(IVehicleRepository vehicleRepository, IVehicleBanRepository vehicleBanRepo,
            IMapper mapper, ILogger<VehicleController> logger)
        {
            _vehicleRepo = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _vehicleBanRepo = vehicleBanRepo ?? throw new ArgumentNullException(nameof(vehicleBanRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ResponseDto = new();
        }

        [HttpGet("{vehicleId}")]
        public async Task<ResponseDto> GetById(int vehicleId)
        {
            _logger.LogInformation($"Executing {nameof(VehicleController)}>{nameof(GetById)}...");

            try
            {
                var vehicleInfo = await _vehicleRepo.GetAsync(d => d.Id == vehicleId);
                if (vehicleInfo == null)
                {
                    _logger.LogInformation($"{nameof(VehicleController)}>{nameof(GetById)} {vehicleId} : Not found!");
                    ResponseDto.Message = "Not found!";
                    ResponseDto.IsSuccess = true;
                    return ResponseDto;
                }

                _logger.LogInformation($"{nameof(VehicleController)}>{nameof(GetById)} {vehicleId} : Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = vehicleInfo;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(VehicleController)}>{nameof(GetById)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpGet]
        public async Task<ResponseDto> GetAllVehicles(GetVehicleRequestDto requestDto)
        {
            _logger.LogInformation($"Executing {nameof(VehicleController)}>{nameof(GetAllVehicles)}...");

            try
            {
                Expression<Func<Entities.Vehicle, bool>> filters = v =>
                    (requestDto.Id == null || v.Id == requestDto.Id) &&
                    (requestDto.PlateNumber == null || v.PlateNumber.Contains(requestDto.PlateNumber)) &&
                    (requestDto.Type == null || v.Type.Contains(requestDto.Type)) &&
                    (requestDto.DriverId == null || v.DriverId == requestDto.DriverId) &&
                    (requestDto.IsActive == null || v.IsActive == requestDto.IsActive) &&
                    (requestDto.CreatedBy == null || v.CreatedBy == requestDto.CreatedBy) &&
                    (requestDto.CreatedStartDate == null || v.CreatedDate >= requestDto.CreatedStartDate) &&
                    (requestDto.CreatedEndDate == null || v.CreatedDate <= requestDto.CreatedEndDate) &&
                    (requestDto.LastModifiedBy == null || v.LastModifiedBy == requestDto.LastModifiedBy) &&
                    (requestDto.LastModifiedStartDate == null || v.LastModifiedDate >= requestDto.LastModifiedStartDate) &&
                    (requestDto.LastModifiedEndDate == null || v.LastModifiedDate <= requestDto.LastModifiedEndDate);

                if (string.IsNullOrEmpty(requestDto.SearchString))
                {
                    Expression<Func<Entities.Vehicle, bool>> searchFilter = v =>
                        (requestDto.SearchString == null ||
                         (v.PlateNumber.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (v.Type.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (v.CreatedBy.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (v.LastModifiedBy.ToLower().Contains(requestDto.SearchString.ToLower())));

                    filters = filters.AndAlso(searchFilter);
                }

                var vehicles = await _vehicleRepo.GetAllAsync(
                    filter: filters,
                    pageSize: requestDto.PageSize,
                    pageNumber: requestDto.PageNumber);

                _logger.LogInformation($"Get vehicles : {vehicles.Count()} Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = vehicles;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(VehicleController)}>{nameof(GetAllVehicles)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> CreateVehicle(CreateVehicleRequestDto requestDto)
        {
            _logger.LogInformation("Creating Vehicle...");
            try
            {
                var vehicleExists = await _vehicleRepo.AnyAsync(v =>
                    v != null
                    && (v.PlateNumber.ToUpper() == requestDto.PlateNumber.ToUpper()
                        || v.DriverId == requestDto.DriverId));
                if (vehicleExists)
                {
                    _logger.LogInformation($"Creating Vehicle Failed. Vehicle with the same plate number and/or driver id has already existed!");
                    ResponseDto.Message = "Vehicle with the same plate number and/or driver id has already existed!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                var createDto = _mapper.Map<Entities.Vehicle>(requestDto);
                createDto.LastModifiedBy = requestDto.ModifiedBy ?? "N/A";
                createDto.CreatedBy = requestDto.ModifiedBy ?? "N/A";

                createDto = await _vehicleRepo.CreateAsync(createDto);
                ResponseDto.Data = createDto;
                ResponseDto.IsSuccess = true;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(VehicleController)}>{nameof(CreateVehicle)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }
            return ResponseDto;
        }

        [HttpPut]
        public async Task<ResponseDto> EditVehicle(EditVehicleRequestDto requestDto)
        {
            _logger.LogInformation("Editing Vehicle...");
            try
            {
                var vehicleExists = await _vehicleRepo.AnyAsync(v =>
                    v != null
                    && (v.PlateNumber.ToUpper() == requestDto.PlateNumber.ToUpper()
                        || v.DriverId == requestDto.DriverId));
                if (!vehicleExists)
                {
                    _logger.LogInformation($"Editing Vehicle Failed. Vehicle does not exist!");
                    ResponseDto.Message = "Vehicle does not exist!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                var vehicle = await _vehicleRepo.GetAsync(v => v.Id == requestDto.Id);
                if (vehicle == null)
                {
                    _logger.LogInformation($"Editing Vehicle Failed. Vehicle not found!");
                    ResponseDto.Message = "Vehicle not found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                _mapper.Map(requestDto, vehicle);
                vehicle.LastModifiedDate = DateTime.UtcNow;
                vehicle.LastModifiedBy = requestDto.ModifiedBy;

                await _vehicleRepo.UpdateAsync(vehicle);
                ResponseDto.Data = vehicle;
                ResponseDto.IsSuccess = true;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(VehicleController)}>{nameof(EditVehicle)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }
            return ResponseDto;
        }

        [HttpGet("bans")]
        public async Task<ResponseDto> GetAllVehicleBans(GetVehicleBanRequestDto requestDto)
        {
            _logger.LogInformation($"Getting vehicle bans...");

            try
            {
                Expression<Func<VehicleBan, bool>> filters = v =>
                    (requestDto.VehicleId == null || v.VehicleId == requestDto.VehicleId) &&
                    (requestDto.Reason == null || v.Reason.Contains(requestDto.Reason)) &&
                    (requestDto.StartTime == null || v.StartTime >= requestDto.StartTime) &&
                    (requestDto.EndTime == null || v.EndTime <= requestDto.EndTime) &&
                    (requestDto.IsActive == null || v.IsActive == requestDto.IsActive) &&
                    (requestDto.CreatedBy == null || v.CreatedBy == requestDto.CreatedBy) &&
                    (requestDto.CreatedStartDate == null ||
                     v.CreatedDate >= requestDto.CreatedStartDate) &&
                    (requestDto.CreatedEndDate == null || v.CreatedDate <= requestDto.CreatedEndDate) &&
                    (requestDto.LastModifiedBy == null || v.LastModifiedBy == requestDto.LastModifiedBy) &&
                    (requestDto.LastModifiedStartDate == null ||
                     v.LastModifiedDate >= requestDto.LastModifiedStartDate) &&
                    (requestDto.LastModifiedEndDate == null ||
                     v.LastModifiedDate <= requestDto.LastModifiedEndDate);

                if (string.IsNullOrEmpty(requestDto.SearchReason))
                {
                    Expression<Func<VehicleBan, bool>> searchFilter = v =>
                        (requestDto.SearchReason == null ||
                         (v.Reason.ToLower().Contains(requestDto.SearchReason.ToLower())));

                    filters = filters.AndAlso(searchFilter);
                }

                var bans = await _vehicleBanRepo.GetAllAsync(
                    filter: filters,
                    pageSize: requestDto.PageSize,
                    pageNumber: requestDto.PageNumber);

                _logger.LogInformation($"Get vehicle bans : {bans.Count()} Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = bans;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(VehicleController)}>{nameof(GetAllVehicleBans)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost("{vehicleId}/ban")]
        public async Task<ResponseDto> BanByVehicleId(int vehicleId, [FromBody] CreateVehicleBanRequestDto requestDto)
        {
            _logger.LogInformation("Execute BanByVehicleId...");

            try
            {
                var vehicleInfo = await _vehicleRepo.GetAsync(v => v.Id == vehicleId);
                if (vehicleInfo == null)
                {
                    _logger.LogInformation($"Executing BanByVehicleId Failed. Vehicle not found!");
                    ResponseDto.Message = "Vehicle not found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                VehicleBan? createBanDto = new VehicleBan();
                createBanDto.VehicleId = vehicleId;
                createBanDto.StartTime = requestDto.StartTime;
                createBanDto.EndTime = requestDto.EndTime;
                createBanDto.Reason = requestDto.Reason;
                createBanDto.CreatedBy = requestDto.ModifiedBy ?? "N/A";
                createBanDto.LastModifiedBy = requestDto.ModifiedBy ?? "N/A";

                createBanDto = await _vehicleBanRepo.CreateAsync(createBanDto);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = createBanDto;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(VehicleController)}>{nameof(BanByVehicleId)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPut("bans/{banId}")]
        public async Task<ResponseDto> EditBanByVehicleId(int bandId, [FromBody] EditVehicleBanRequestDto requestDto)
        {
            _logger.LogInformation("Execute EditBanByVehicleId...");

            try
            {
                var vehicleBan = await _vehicleBanRepo.GetAsync(v => v.Id == bandId);
                if (vehicleBan == null)
                {
                    _logger.LogInformation($"Executing EditBanByVehicleId Failed. Not found!");
                    ResponseDto.Message = "Not found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                vehicleBan.StartTime = requestDto.StartTime;
                vehicleBan.EndTime = requestDto.EndTime;
                vehicleBan.Reason = requestDto.Reason;
                vehicleBan.IsActive = requestDto.IsActive;
                vehicleBan.LastModifiedDate = DateTime.UtcNow;
                vehicleBan.LastModifiedBy = requestDto.ModifiedBy ?? "N/A";

                vehicleBan = await _vehicleBanRepo.UpdateAsync(vehicleBan);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = vehicleBan;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(VehicleController)}>{nameof(EditBanByVehicleId)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPut("bans/{banId}/unban")]
        public async Task<ResponseDto> UnbanByVehicleId(int banId)
        {
            _logger.LogInformation("Execute UnbanByVehicleId...");

            try
            {
                var vehicleBan = await _vehicleBanRepo.GetAsync(v => v.Id == banId);
                if (vehicleBan == null)
                {
                    _logger.LogInformation($"Executing UnbanByVehicleId Failed. Not found!");
                    ResponseDto.Message = "Not found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                vehicleBan.IsActive = false;
                vehicleBan = await _vehicleBanRepo.UpdateAsync(vehicleBan);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = vehicleBan;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(VehicleController)}>{nameof(UnbanByVehicleId)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }
    }
}
