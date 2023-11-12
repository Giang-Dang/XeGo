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
        public async Task<ResponseDto> GetAll(GetDriverInfoRequestDto requestDto)
        {
            _logger.LogInformation($"Getting drivers...");

            try
            {
                Expression<Func<DriverInfo, bool>> filters = d =>
                    (requestDto.DriverId == null || d.UserId == requestDto.DriverId) &&
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
                         (d.UserId.Contains(requestDto.SearchString)) ||
                         (d.FirstName.Contains(requestDto.SearchString)) ||
                         (d.LastName.Contains(requestDto.SearchString)) ||
                         (d.Email.Contains(requestDto.SearchString)) ||
                         (d.PhoneNumber.Contains(requestDto.SearchString)) ||
                         (d.Address.Contains(requestDto.SearchString)) ||
                         (d.District.Contains(requestDto.SearchString)) ||
                         (d.City.Contains(requestDto.SearchString)) ||
                         (d.CreatedBy.Contains(requestDto.SearchString)) ||
                         (d.LastModifiedBy.Contains(requestDto.SearchString)));

                    filters = filters.AndAlso(searchFilter);
                }

                var drivers = await _driverInfoRepo.GetAllAsync(
                    filter: filters,
                    pageSize: requestDto.PageSize,
                    pageNumber: requestDto.PageNumber);

                _logger.LogInformation($"Get drivers : Found {drivers.Count()} drivers!");
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
        public async Task<ResponseDto> Create(CreateDriverInfoRequestDto requestDto)
        {
            _logger.LogInformation("Creating Driver Info...");
            try
            {
                var driverExists = await _driverInfoRepo.AnyAsync(d =>
                    d != null
                    && (d.Email.ToUpper() == requestDto.Email.ToUpper()
                        || d.PhoneNumber == requestDto.PhoneNumber));
                if (driverExists)
                {
                    _logger.LogInformation($"Creating Driver Info Failed. Driver with the same phone number and/or email has already existed!");
                    ResponseDto.Message = "Driver with the same phone number and/or email has already existed!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                var createDto = _mapper.Map<DriverInfo>(requestDto);
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
                    && (d.Email.ToUpper() == requestDto.Email.ToUpper()
                        || d.PhoneNumber == requestDto.PhoneNumber));
                if (!driverExists)
                {
                    _logger.LogInformation($"Editing Driver Info Failed. Driver does not exist!");
                    ResponseDto.Message = "Driver does not exist!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                var driverInfo = await _driverInfoRepo.GetAsync(d => d.UserId == requestDto.DriverId);
                if (driverInfo == null)
                {
                    _logger.LogInformation($"Editing Driver Info Failed. Driver not found!");
                    ResponseDto.Message = "Driver not found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                _mapper.Map(requestDto, driverInfo);
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

    }
}
