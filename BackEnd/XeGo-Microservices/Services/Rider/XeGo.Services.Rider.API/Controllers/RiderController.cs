using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using XeGo.Services.Rider.API.Entities;
using XeGo.Services.Rider.API.Models;
using XeGo.Services.Rider.API.Repository.IRepository;
using XeGo.Shared.Lib.Helpers;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Rider.API.Controllers
{
    [Route("api/riders")]
    [ApiController]
    public class RiderController : ControllerBase
    {
        private readonly IRiderInfoRepository _riderInfoRepo;
        private readonly IRiderBanRepository _riderBanRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<RiderController> _logger;
        private ResponseDto ResponseDto { get; set; }

        public RiderController(IRiderInfoRepository riderInfoRepo, IRiderBanRepository riderBanRepo, IMapper mapper,
            ILogger<RiderController> logger)
        {
            _riderInfoRepo = riderInfoRepo ?? throw new ArgumentNullException(nameof(riderInfoRepo));
            _riderBanRepo = riderBanRepo ?? throw new ArgumentNullException(nameof(riderBanRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ResponseDto = new();
        }

        [HttpGet("{riderId}")]
        public async Task<ResponseDto> GetRiderById(string riderId)
        {
            _logger.LogInformation($"Getting rider by Id {riderId}...");

            try
            {
                var riderInfo = await _riderInfoRepo.GetAsync(r => r.UserId == riderId);
                if (riderInfo == null)
                {
                    _logger.LogInformation($"Get rider by Id {riderId} : Not found!");
                    ResponseDto.Message = "Not found!";
                    ResponseDto.IsSuccess = true;
                    return ResponseDto;
                }

                _logger.LogInformation($"Get rider by Id {riderId} : Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = riderInfo;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(RiderController)}>{nameof(GetRiderById)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpGet]
        public async Task<ResponseDto> GetAllRiders(GetRiderInfoRequestDto requestDto)
        {
            _logger.LogInformation($"Getting riders...");

            try
            {
                Expression<Func<RiderInfo, bool>> filters = r =>
                    (requestDto.RiderId == null || r.UserId == requestDto.RiderId) &&
                    (requestDto.FirstName == null || r.FirstName.Contains(requestDto.FirstName)) &&
                    (requestDto.LastName == null || r.LastName.Contains(requestDto.LastName)) &&
                    (requestDto.Email == null || r.Email.Contains(requestDto.Email)) &&
                    (requestDto.PhoneNumber == null || r.PhoneNumber.Contains(requestDto.PhoneNumber)) &&
                    (requestDto.Address == null || r.Address.Contains(requestDto.Address)) &&
                    (requestDto.District == null || r.District.Contains(requestDto.District)) &&
                    (requestDto.City == null || r.City.Contains(requestDto.City)) &&
                    (requestDto.CreatedBy == null || r.CreatedBy == requestDto.CreatedBy) &&
                    (requestDto.CreatedStartDate == null ||
                     r.CreatedDate >= requestDto.CreatedStartDate) &&
                    (requestDto.CreatedEndDate == null || r.CreatedDate <= requestDto.CreatedEndDate) &&
                    (requestDto.LastModifiedBy == null || r.LastModifiedBy == requestDto.LastModifiedBy) &&
                    (requestDto.LastModifiedStartDate == null ||
                     r.LastModifiedDate >= requestDto.LastModifiedStartDate) &&
                    (requestDto.LastModifiedEndDate == null ||
                     r.LastModifiedDate <= requestDto.LastModifiedEndDate);

                if (string.IsNullOrEmpty(requestDto.SearchString))
                {
                    Expression<Func<RiderInfo, bool>> searchFilter = r =>
                        (requestDto.SearchString == null ||
                         (r.UserId.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (r.FirstName.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (r.LastName.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (r.Email.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (r.PhoneNumber.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (r.Address.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (r.District.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (r.City.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (r.CreatedBy.ToLower().Contains(requestDto.SearchString.ToLower())) ||
                         (r.LastModifiedBy.ToLower().Contains(requestDto.SearchString.ToLower())));

                    filters = filters.AndAlso(searchFilter);
                }

                var riders = await _riderInfoRepo.GetAllAsync(
                    filter: filters,
                    pageSize: requestDto.PageSize,
                    pageNumber: requestDto.PageNumber);

                _logger.LogInformation($"Get riders : {riders.Count()} Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = riders;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(RiderController)}>{nameof(GetAllRiders)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> CreateRider(CreateRiderInfoRequestDto requestDto)
        {
            _logger.LogInformation("Creating Rider Info...");
            try
            {
                var riderExists = await _riderInfoRepo.AnyAsync(r =>
                    r != null
                    && r.UserId == requestDto.UserId);
                if (riderExists)
                {
                    _logger.LogInformation($"Creating Rider Info Failed. Rider with the same phone number and/or email has already existed!");
                    ResponseDto.Message = "Rider with the same phone number and/or email has already existed!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                var createDto = _mapper.Map<RiderInfo>(requestDto);
                createDto.LastModifiedBy = requestDto.ModifiedBy ?? "N/A";
                createDto.CreatedBy = requestDto.ModifiedBy ?? "N/A";

                createDto = await _riderInfoRepo.CreateAsync(createDto);
                ResponseDto.Data = createDto;
                ResponseDto.IsSuccess = true;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(RiderController)}>{nameof(CreateRider)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }
            return ResponseDto;
        }

        [HttpPut]
        public async Task<ResponseDto> EditRider(EditRiderInfoRequestDto requestDto)
        {
            _logger.LogInformation("Editing Rider Info...");
            try
            {
                var riderExists = await _riderInfoRepo.AnyAsync(r =>
                    r != null
                    && r.UserId == requestDto.UserId);
                if (!riderExists)
                {
                    _logger.LogInformation($"Editing Rider Info Failed. Rider does not exist!");
                    ResponseDto.Message = "Rider does not exist!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                var riderInfo = await _riderInfoRepo.GetAsync(r => r.UserId == requestDto.UserId);
                if (riderInfo == null)
                {
                    _logger.LogInformation($"Editing Rider Info Failed. Rider not found!");
                    ResponseDto.Message = "Rider not found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                _mapper.Map(requestDto, riderInfo);
                riderInfo.LastModifiedDate = DateTime.UtcNow;
                riderInfo.LastModifiedBy = requestDto.ModifiedBy;

                await _riderInfoRepo.UpdateAsync(riderInfo);
                ResponseDto.Data = riderInfo;
                ResponseDto.IsSuccess = true;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(RiderController)}>{nameof(EditRider)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }
            return ResponseDto;
        }

        [HttpGet("bans")]
        public async Task<ResponseDto> GetAllRiderBans(GetRiderBanRequestDto requestDto)
        {
            _logger.LogInformation($"Getting rider bans...");

            try
            {
                Expression<Func<RiderBan, bool>> filters = r =>
                    (requestDto.RiderId == null || r.RiderId == requestDto.RiderId) &&
                    (requestDto.Reason == null || r.Reason.Contains(requestDto.Reason)) &&
                    (requestDto.StartTime == null || r.StartTime >= requestDto.StartTime) &&
                    (requestDto.EndTime == null || r.EndTime <= requestDto.EndTime) &&
                    (requestDto.IsActive == null || r.IsActive == requestDto.IsActive) &&
                    (requestDto.CreatedBy == null || r.CreatedBy == requestDto.CreatedBy) &&
                    (requestDto.CreatedStartDate == null ||
                     r.CreatedDate >= requestDto.CreatedStartDate) &&
                    (requestDto.CreatedEndDate == null || r.CreatedDate <= requestDto.CreatedEndDate) &&
                    (requestDto.LastModifiedBy == null || r.LastModifiedBy == requestDto.LastModifiedBy) &&
                    (requestDto.LastModifiedStartDate == null ||
                     r.LastModifiedDate >= requestDto.LastModifiedStartDate) &&
                    (requestDto.LastModifiedEndDate == null ||
                     r.LastModifiedDate <= requestDto.LastModifiedEndDate);

                if (string.IsNullOrEmpty(requestDto.SearchReason))
                {
                    Expression<Func<RiderBan, bool>> searchFilter = r =>
                        (requestDto.SearchReason == null ||
                         (r.Reason.ToLower().Contains(requestDto.SearchReason.ToLower())));

                    filters = filters.AndAlso(searchFilter);
                }

                var bans = await _riderBanRepo.GetAllAsync(
                    filter: filters,
                    pageSize: requestDto.PageSize,
                    pageNumber: requestDto.PageNumber);

                _logger.LogInformation($"Get rider bans : {bans.Count()} Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = bans;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(RiderController)}>{nameof(GetAllRiderBans)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost("{riderId}/ban")]
        public async Task<ResponseDto> BanByRiderId(string riderId, [FromBody] CreateRiderBanRequestDto requestDto)
        {
            _logger.LogInformation("Execute BanByRiderId...");

            try
            {
                var riderInfo = await _riderInfoRepo.GetAsync(r => r.UserId == riderId);
                if (riderInfo == null)
                {
                    _logger.LogInformation($"Executing BanByRiderId Failed. Rider not found!");
                    ResponseDto.Message = "Rider not found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                RiderBan? createBanDto = new RiderBan();
                createBanDto.RiderId = riderId;
                createBanDto.StartTime = requestDto.StartTime;
                createBanDto.EndTime = requestDto.EndTime;
                createBanDto.Reason = requestDto.Reason;
                createBanDto.CreatedBy = requestDto.ModifiedBy ?? "N/A";
                createBanDto.LastModifiedBy = requestDto.ModifiedBy ?? "N/A";

                createBanDto = await _riderBanRepo.CreateAsync(createBanDto);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = createBanDto;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(RiderController)}>{nameof(UnbanByRiderId)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPut("bans/{banId}")]
        public async Task<ResponseDto> EditBanByRiderId(int banId, [FromBody] EditRiderBanRequestDto requestDto)
        {
            _logger.LogInformation("Execute EditBanByRiderId...");

            try
            {
                var riderBan = await _riderBanRepo.GetAsync(r => r.Id == banId);
                if (riderBan == null)
                {
                    _logger.LogInformation($"Executing BanByRiderId Failed. Not found!");
                    ResponseDto.Message = "Not found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                riderBan.StartTime = requestDto.StartTime ?? riderBan.StartTime;
                riderBan.EndTime = requestDto.EndTime ?? riderBan.EndTime;
                riderBan.Reason = requestDto.Reason ?? riderBan.Reason;
                riderBan.IsActive = requestDto.IsActive ?? riderBan.IsActive;
                riderBan.LastModifiedDate = DateTime.UtcNow;
                riderBan.LastModifiedBy = requestDto.ModifiedBy ?? "N/A";

                riderBan = await _riderBanRepo.UpdateAsync(riderBan);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = riderBan;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(RiderController)}>{nameof(EditBanByRiderId)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPut("bans/{banId}/unban")]
        public async Task<ResponseDto> UnbanByRiderId(int banId)
        {
            _logger.LogInformation("Execute BanByRiderId...");

            try
            {
                var riderBan = await _riderBanRepo.GetAsync(r => r.Id == banId);
                if (riderBan == null)
                {
                    _logger.LogInformation($"Executing UnbanByRiderId Failed. Not found!");
                    ResponseDto.Message = "Not found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                riderBan.IsActive = false;
                riderBan = await _riderBanRepo.UpdateAsync(riderBan);
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = riderBan;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(RiderController)}>{nameof(UnbanByRiderId)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

    }
}
