using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XeGo.Services.Location.API.Data;
using XeGo.Services.Location.API.Entities;
using XeGo.Services.Location.API.Models.Dto;
using XeGo.Services.Location.API.Services.IServices;
using XeGo.Shared.Lib.Constants;
using XeGo.Shared.Lib.Helpers;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Location.API.Controllers
{
    [Route("api/locations/drivers")]
    [ApiController]
    public class DriverLocationController(
        AppDbContext dbContext,
        IMapper mapper,
        ILogger<DriverLocationController> logger,
        IGeoHashService geoHashService) : ControllerBase
    {
        private readonly AppDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly ILogger<DriverLocationController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IGeoHashService _geoHashService = geoHashService;
        private ResponseDto ResponseDto { get; set; } = new();

        [HttpGet]
        public async Task<ResponseDto> GetLocationByUserId(string userId)
        {
            try
            {
                var userLocationRes = await _dbContext.DriverLocations.FirstOrDefaultAsync(e => e.UserId == userId);
                if (userLocationRes == null)
                {
                    ResponseDto.IsSuccess = true;
                    ResponseDto.Data = null;
                    ResponseDto.Message = "Location is not found";
                }

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = userLocationRes;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(DriverLocationController)}>{nameof(GetLocationByUserId)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> PushLocation([FromBody] RiderLocationRequestDto requestDto)
        {
            _logger.LogInformation($"{nameof(DriverLocationController)}>{nameof(PushLocation)}: Triggered!");

            try
            {
                if (!_geoHashService.IsValidCoordinates(requestDto.Latitude, requestDto.Longitude))
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = "Invalid Coordinate!";
                    _logger.LogInformation($"{nameof(DriverLocationController)}>{nameof(PushLocation)}: Invalid Coordinate!");
                    return ResponseDto;
                }

                var cGeoSquareSideInMeters = await dbContext.CodeValues.FirstOrDefaultAsync(c => c.Name == GeohashConstants.GeohashName && c.Value1 == GeohashConstants.GeoHashSquareSideInMeters);
                if (cGeoSquareSideInMeters == null)
                {
                    _logger.LogError($"{nameof(DriverLocationController)}>{nameof(PushLocation)}: {GeohashConstants.GeoHashSquareSideInMeters} Not Found!");
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = $"{GeohashConstants.GeoHashSquareSideInMeters} Not Found!!";
                    return ResponseDto;
                }

                double geoSquareSideInMeters = CodeValueHelpers
                    .GetOriginalValue(
                        cGeoSquareSideInMeters.Value2 ?? "500",
                        cGeoSquareSideInMeters.Value2Type ?? CodeValueTypeConstants.Double);

                var geoHash = _geoHashService.Geohash(requestDto.Latitude, requestDto.Longitude, geoSquareSideInMeters);
                var userLocationRes =
                    await _dbContext.DriverLocations.FirstOrDefaultAsync(e => e.UserId == requestDto.UserId);

                if (userLocationRes == null)
                {
                    //Create
                    DriverLocation createEntity = new();
                    _mapper.Map(requestDto, createEntity);

                    createEntity.Geohash = geoHash;
                    createEntity.CreatedBy = requestDto.CreatedBy ?? requestDto.ModifiedBy;
                    createEntity.LastModifiedBy = requestDto.ModifiedBy;

                    await _dbContext.DriverLocations.AddAsync(createEntity);
                    await _dbContext.SaveChangesAsync();

                    ResponseDto.Message = "Location created";
                    ResponseDto.IsSuccess = true;
                    ResponseDto.Data = null;

                    _logger.LogInformation($"{nameof(DriverLocationController)}>{nameof(PushLocation)}: Done!");

                    return ResponseDto;
                }

                //Update
                userLocationRes.Geohash = geoHash;
                userLocationRes.LastModifiedBy = requestDto.ModifiedBy;
                userLocationRes.Latitude = requestDto.Latitude;
                userLocationRes.Longitude = requestDto.Longitude;

                ResponseDto.Message = "Location updated";
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = null;

                _dbContext.DriverLocations.Update(userLocationRes);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"{nameof(DriverLocationController)}>{nameof(PushLocation)}: Done!");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(DriverLocationController)}>{nameof(PushLocation)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpDelete("{userId}")]
        public async Task<ResponseDto> DeleteLocation(string userId)
        {
            _logger.LogInformation($"{nameof(DriverLocationController)}>{nameof(DeleteLocation)}: Triggered!");

            try
            {
                var userLocationRes = await _dbContext.DriverLocations.FirstOrDefaultAsync(e => e.UserId == userId);

                if (userLocationRes == null)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = "Driver location not found!";
                    _logger.LogInformation($"{nameof(DriverLocationController)}>{nameof(DeleteLocation)}: Driver location not found!");
                    return ResponseDto;
                }

                _dbContext.DriverLocations.Remove(userLocationRes);
                await _dbContext.SaveChangesAsync();

                ResponseDto.Message = "Location deleted";
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = null;

                _logger.LogInformation($"{nameof(DriverLocationController)}>{nameof(DeleteLocation)}: Done!");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(DriverLocationController)}>{nameof(DeleteLocation)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }
    }
}
