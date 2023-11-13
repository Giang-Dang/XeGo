using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using XeGo.Services.Driver.API.Entities;
using XeGo.Services.Driver.API.Models;
using XeGo.Services.Driver.API.Repository.IRepository;
using XeGo.Shared.Lib.Helpers;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Driver.API.Controllers
{
    [Route("api/drivers")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverInfoRepository _driverInfoRepo;
        private readonly IDriverBanRepository _driverBanRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<DriverController> _logger;
        private ResponseDto ResponseDto { get; set; }

        public DriverController(IDriverInfoRepository driverInfoRepo, IDriverBanRepository driverBanRepo,
            IMapper mapper, ILogger<DriverController> logger)
        {
            _driverInfoRepo = driverInfoRepo ?? throw new ArgumentNullException(nameof(driverInfoRepo));
            _driverBanRepo = driverBanRepo ?? throw new ArgumentNullException(nameof(driverBanRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ResponseDto = new();
        }

        [HttpGet("{driverId}")]
        public async Task<ResponseDto> GetById(string driverId)
        {
            _logger.LogInformation($"Getting driver by Id {driverId}...");

            try
            {
                var driverInfo = await _driverInfoRepo.GetAsync(d => d.UserId == driverId);
                if (driverInfo == null)
                {
                    _logger.LogInformation($"Get driver by Id {driverId} : Not found!");
                    ResponseDto.Message = "Not found!";
                    ResponseDto.IsSuccess = true;
                    return ResponseDto;
                }

                _logger.LogInformation($"Get driver by Id {driverId} : Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = driverInfo;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(DriverController)}>{nameof(GetById)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpGet]
        public async Task<ResponseDto> GetAllDrivers(GetDriverInfoRequestDto requestDto)
        {
            _logger.LogInformation($"Getting drivers...");

            try
            {
                Expression<Func<DriverInfo, bool>> filters = d =>
                    (requestDto.UserId == null || d.UserId == requestDto.UserId) &&
                    (requestDto.FirstName == null || d.FirstName.Contains(requestDto.FirstName)) &&
                    (requestDto.LastName == null || d.LastName.Contains(requestDto.LastName)) &&
                    (requestDto.Email == null || d.Email.Contains(requestDto.Email)) &&
                    (requestDto.PhoneNumber == null || d.PhoneNumber.Contains(requestDto.PhoneNumber)) &&
                    (requestDto.Address == null || d.Address.Contains(requestDto.Address)) &&
                    (requestDto.District == null || d.District.Contains(requestDto.District)) &&
                    (requestDto.City == null || d.City.Contains(requestDto.City)) &&
                    (requestDto.CreatedBy == null || d.CreatedBy == requestDto.CreatedBy) &&
                    (requestDto.CreatedStartDate == null ||
                     d.CreatedDate >= requestDto.CreatedStartDate) &&
                    (requestDto.CreatedEndDate == null || d.CreatedDate <= requestDto.CreatedEndDate) &&
                    (requestDto.LastModifiedBy == null || d.LastModifiedBy == requestDto.LastModifiedBy) &&
                    (requestDto.LastModifiedStartDate == null ||
                     d.LastModifiedDate >= requestDto.LastModifiedStartDate) &&
                    (requestDto.LastModifiedEndDate == null ||
                     d.LastModifiedDate <= requestDto.LastModifiedEndDate);

                if (string.IsNullOrEmpty(requestDto.SearchString))
                {
                    Expression<Func<DriverInfo, bool>> searchFilter = d =>
                        (requestDto.SearchString == null ||
                         (d.UserId.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (d.FirstName.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (d.LastName.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (d.Email.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (d.PhoneNumber.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (d.Address.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (d.District.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (d.City.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (d.CreatedBy.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (d.LastModifiedBy.ToLower().Contains(requestDto.SearchString.ToLower())));

                    filters = filters.AndAlso(searchFilter);
                }

                var drivers = await _driverInfoRepo.GetAllAsync(
                    filter: filters,
                    pageSize: requestDto.PageSize,
                    pageNumber: requestDto.PageNumber);

                _logger.LogInformation($"Get drivers : {drivers.Count()} Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = drivers;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(DriverController)}>{nameof(GetAll)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> CreateDriver(CreateDriverInfoRequestDto requestDto)
        {
            _logger.LogInformation("Creating Driver Info...");
            try
            {
                var driverExists = await _driverInfoRepo.AnyAsync(d =>
                    d != null
                    && d.UserId == requestDto.UserId);
                if (driverExists)
                {
                    _logger.LogInformation($"Creating Driver Info Failed. Driver with the same phone number and/or email has already existed!");
                    ResponseDto.Message = "Driver with the same phone number and/or email has already existed!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                var createDto = _mapper.Map<DriverInfo>(requestDto);
                createDto.LastModifiedBy = requestDto.ModifiedBy ?? "N/A";
                createDto.CreatedBy = requestDto.ModifiedBy ?? "N/A";

                createDto = await _driverInfoRepo.CreateAsync(createDto);
                ResponseDto.Data = createDto;
                ResponseDto.IsSuccess = true;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(DriverController)}>{nameof(Create)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }
            return ResponseDto;
        }

        [HttpPut]
        public async Task<ResponseDto> Edit(EditDriverInfoRequestDto requestDto)
        {
            _logger.LogInformation("Editing Driver Info...");
            try
            {
                var driverExists = await _driverInfoRepo.AnyAsync(d =>
                    d != null
                    && d.UserId == requestDto.UserId);
                if (!driverExists)
                {
                    _logger.LogInformation($"Editing Driver Info Failed. Driver does not exist!");
                    ResponseDto.Message = "Driver does not exist!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                var driverInfo = await _driverInfoRepo.GetAsync(d => d.UserId == requestDto.UserId);
                if (driverInfo == null)
                {
                    _logger.LogInformation($"Editing Driver Info Failed. Driver not found!");
                    ResponseDto.Message = "Driver not found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                _mapper.Map(requestDto, driverInfo);
                driverInfo.LastModifiedDate = DateTime.UtcNow;
                driverInfo.LastModifiedBy = requestDto.ModifiedBy;

                await _driverInfoRepo.UpdateAsync(driverInfo);
                ResponseDto.Data = driverInfo;
                ResponseDto.IsSuccess = true;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(DriverController)}>{nameof(Edit)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }
            return ResponseDto;
        }

        [HttpGet("bans")]
        public async Task<ResponseDto> GetAllBans(GetDriverBanRequestDto requestDto)
        {
            _logger.LogInformation($"Getting driver bans...");

            try
            {
                Expression<Func<DriverBan, bool>> filters = d =>
                    (requestDto.DriverId == null || d.DriverId == requestDto.DriverId) &&
                    (requestDto.Reason == null || d.Reason.Contains(requestDto.Reason)) &&
                    (requestDto.StartTime == null || d.StartTime >= requestDto.StartTime) &&
                    (requestDto.EndTime == null || d.EndTime <= requestDto.EndTime) &&
                    (requestDto.IsActive == null || d.IsActive == requestDto.IsActive) &&
                    (requestDto.CreatedBy == null || d.CreatedBy == requestDto.CreatedBy) &&
                    (requestDto.CreatedStartDate == null ||
                     d.CreatedDate >= requestDto.CreatedStartDate) &&
                    (requestDto.CreatedEndDate == null || d.CreatedDate <= requestDto.CreatedEndDate) &&
                    (requestDto.LastModifiedBy == null || d.LastModifiedBy == requestDto.LastModifiedBy) &&
                    (requestDto.LastModifiedStartDate == null ||
                     d.LastModifiedDate >= requestDto.LastModifiedStartDate) &&
                    (requestDto.LastModifiedEndDate == null ||
                     d.LastModifiedDate <= requestDto.LastModifiedEndDate);

                if (string.IsNullOrEmpty(requestDto.SearchReason))
                {
                    Expression<Func<DriverBan, bool>> searchFilter = d =>
                        (requestDto.SearchReason == null ||
                         (d.Reason.ToLower().Contains(requestDto.SearchReason.ToLower())));

                    filters = filters.AndAlso(searchFilter);
                }

                var bans = await _driverBanRepo.GetAllAsync(
                    filter: filters,
                    pageSize: requestDto.PageSize,
                    pageNumber: requestDto.PageNumber);

                _logger.LogInformation($"Get driver bans : {bans.Count()} Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = bans;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(DriverController)}>{nameof(GetAllBans)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost("{driverId}/ban")]
        public async Task<ResponseDto> BanByDriverId(string driverId, [FromBody] CreateDriverBanRequestDto requestDto)
        {
            _logger.LogInformation("Execute BanByDriverId...");

            try
            {
                var driverInfo = await _driverInfoRepo.GetAsync(d => d.UserId == driverId);
                if (driverInfo == null)
                {
                    _logger.LogInformation($"Executing BanByDriverId Failed. Driver not found!");
                    ResponseDto.Message = "Driver not found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                DriverBan? createBanDto = new DriverBan();
                createBanDto.DriverId = driverId;
                createBanDto.StartTime = requestDto.StartTime;
                createBanDto.EndTime = requestDto.EndTime;
                createBanDto.Reason = requestDto.Reason;
                createBanDto.CreatedBy = requestDto.ModifiedBy ?? "N/A";
                createBanDto.LastModifiedBy = requestDto.ModifiedBy ?? "N/A";

                createBanDto = await _driverBanRepo.CreateAsync(createBanDto);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = createBanDto;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(DriverController)}>{nameof(UnbanByDriverId)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPut("bans/{banId}")]
        public async Task<ResponseDto> EditBanByDriverId(int banId, [FromBody] EditDriverBanRequestDto requestDto)
        {
            _logger.LogInformation("Execute EditBanByDriverId...");

            try
            {
                var driverBan = await _driverBanRepo.GetAsync(d => d.Id == banId);
                if (driverBan == null)
                {
                    _logger.LogInformation($"Executing BanByDriverId Failed. Not found!");
                    ResponseDto.Message = "Not found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                driverBan.StartTime = requestDto.StartTime;
                driverBan.EndTime = requestDto.EndTime;
                driverBan.Reason = requestDto.Reason;
                driverBan.IsActive = requestDto.IsActive;
                driverBan.LastModifiedDate = DateTime.UtcNow;
                driverBan.LastModifiedBy = requestDto.ModifiedBy ?? "N/A";

                driverBan = await _driverBanRepo.UpdateAsync(driverBan);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = driverBan;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(DriverController)}>{nameof(EditBanByDriverId)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPut("bans/{banId}/unban")]
        public async Task<ResponseDto> UnbanByDriverId(int banId)
        {
            _logger.LogInformation("Execute BanByDriverId...");

            try
            {
                var driverBan = await _driverBanRepo.GetAsync(d => d.Id == banId);
                if (driverBan == null)
                {
                    _logger.LogInformation($"Executing UnbanByDriverId Failed. Not found!");
                    ResponseDto.Message = "Not found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                driverBan.IsActive = false;
                driverBan = await _driverBanRepo.UpdateAsync(driverBan);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = driverBan;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(DriverController)}>{nameof(UnbanByDriverId)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }
    }
}
