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
    [Route("api/locations/riders")]
    [ApiController]
    public class RiderLocationController(
        AppDbContext dbContext,
        IMapper mapper,
        ILogger<RiderLocationController> logger,
        IGeoHashService geoHashService
        ) : ControllerBase
    {
        private readonly AppDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly ILogger<RiderLocationController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IGeoHashService _geoHashService = geoHashService;
        private ResponseDto ResponseDto { get; set; } = new();

        [HttpGet]
        public async Task<ResponseDto> GetLocationByUserId(string userId)
        {
            try
            {
                var userLocationRes = await _dbContext.RiderLocations.FirstOrDefaultAsync(e => e.UserId == userId);
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
                _logger.LogError($"{nameof(RiderLocationController)}>{nameof(GetLocationByUserId)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> PushLocation([FromBody] RiderLocationRequestDto requestDto)
        {
            _logger.LogInformation($"{nameof(RiderLocationRequestDto)}>{nameof(PushLocation)}: Triggered!");

            try
            {
                if (!_geoHashService.IsValidCoordinates(requestDto.Latitude, requestDto.Longitude))
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = "Invalid Coordinate!";
                    _logger.LogInformation($"{nameof(RiderLocationRequestDto)}>{nameof(PushLocation)}: Invalid Coordinate!");
                    return ResponseDto;
                }

                var cGeoSquareSideInMeters = await dbContext.CodeValues.FirstOrDefaultAsync(c => c.Name == GeohashConstants.GeohashName && c.Value1 == GeohashConstants.GeoHashSquareSideInMeters);
                if (cGeoSquareSideInMeters == null)
                {
                    _logger.LogError($"{nameof(RiderLocationRequestDto)}>{nameof(PushLocation)}: {GeohashConstants.GeoHashSquareSideInMeters} Not Found!");
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
                    await _dbContext.RiderLocations.FirstOrDefaultAsync(e => e.UserId == requestDto.UserId);

                if (userLocationRes == null)
                {
                    //Create
                    RiderLocation createEntity = new()
                    {
                        UserId = requestDto.UserId,
                        Geohash = geoHash,
                        Latitude = requestDto.Latitude,
                        Longitude = requestDto.Longitude,
                        CreatedBy = requestDto.CreatedBy ?? requestDto.ModifiedBy,
                        LastModifiedBy = requestDto.ModifiedBy,
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                    };

                    await _dbContext.RiderLocations.AddAsync(createEntity);
                    await _dbContext.SaveChangesAsync();

                    ResponseDto.Message = "Location created";
                    ResponseDto.IsSuccess = true;
                    ResponseDto.Data = null;

                    _logger.LogInformation($"{nameof(RiderLocationRequestDto)}>{nameof(PushLocation)}: Done!");

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

                _dbContext.RiderLocations.Update(userLocationRes);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"{nameof(RiderLocationRequestDto)}>{nameof(PushLocation)}: Done!");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(RiderLocationRequestDto)}>{nameof(PushLocation)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpDelete("{userId}")]
        public async Task<ResponseDto> DeleteLocation(string userId)
        {
            _logger.LogInformation($"{nameof(RiderLocationController)}>{nameof(DeleteLocation)}: Triggered!");

            try
            {
                var userLocationRes = await _dbContext.RiderLocations.FirstOrDefaultAsync(e => e.UserId == userId);

                if (userLocationRes == null)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = "Rider location not found!";
                    _logger.LogInformation($"{nameof(RiderLocationController)}>{nameof(DeleteLocation)}: Rider location not found!");
                    return ResponseDto;
                }

                _dbContext.RiderLocations.Remove(userLocationRes);
                await _dbContext.SaveChangesAsync();

                ResponseDto.Message = "Location deleted";
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = null;

                _logger.LogInformation($"{nameof(RiderLocationController)}>{nameof(DeleteLocation)}: Done!");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(RiderLocationController)}>{nameof(DeleteLocation)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        #region Private Methods

        //private async Task<double> GetGeoSquareSideMeters()
        //{
        //    var response = await _codeValueGrpcService
        //        .GetValue2(GeohashConstants.GeohashName, GeohashConstants.GeoHashSquareSideInMeters, null, null);

        //    return Convert.ToDouble(response.Data);
        //}

        //private async Task<double> GetRadiusInMeters()
        //{
        //    var response = await _codeValueGrpcService
        //        .GetValue2(GeohashConstants.GeohashName, GeohashConstants.MaxRadiusInMetersName, null, null);

        //    return Convert.ToDouble(response.Data);
        //}

        #endregion

    }
}
