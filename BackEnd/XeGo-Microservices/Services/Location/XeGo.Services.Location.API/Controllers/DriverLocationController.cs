using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XeGo.Services.Location.API.Data;
using XeGo.Services.Location.API.Services.IServices;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Location.API.Controllers
{
    public class DriverLocationController
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<DriverLocationController> _logger;
        private readonly IGeoHashService _geoHashService;
        private ResponseDto ResponseDto { get; set; }

        public DriverLocationController(AppDbContext dbContext, IMapper mapper, ILogger<DriverLocationController> logger, IGeoHashService geoHashService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _geoHashService = geoHashService;
            ResponseDto = new();
        }

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

        //[HttpPost]
        //public async Task<ResponseDto> PushLocation([FromBody] RiderLocationRequestDto requestDto)
        //{
        //    try
        //    {
        //        if (!_geoHashService.IsValidCoordinates(requestDto.Latitude, requestDto.Longitude))
        //        {
        //            ResponseDto.IsSuccess = false;
        //            ResponseDto.Data = null;
        //            ResponseDto.Message = "Invalid Coordinate!";
        //            _logger.LogInformation($"{nameof(DriverLocationController)}>{nameof(PushLocation)}: Invalid Coordinate!");
        //            return ResponseDto;
        //        }

        //        //double geoSquareSideMeters = 500.0;
        //        double geoSquareSideMeters = await GetGeoSquareSideMeters();
        //        var geoHash = _geoHashService.Geohash(requestDto.Latitude, requestDto.Longitude, geoSquareSideMeters);
        //        var userLocationRes =
        //            await _dbContext.DriverLocations.FirstOrDefaultAsync(e => e.UserId == requestDto.UserId);

        //        if (userLocationRes == null)
        //        {
        //            DriverLocation createEntity = new();
        //            _mapper.Map(requestDto, createEntity);

        //            createEntity.Geohash = geoHash;
        //            createEntity.CreatedBy = requestDto.CreatedBy ?? requestDto.ModifiedBy;
        //            createEntity.LastModifiedBy = requestDto.ModifiedBy;

        //            await _dbContext.DriverLocations.AddAsync(createEntity);
        //            await _dbContext.SaveChangesAsync();

        //            ResponseDto.Message = "Location created";
        //            ResponseDto.IsSuccess = true;
        //            ResponseDto.Data = null;

        //            return ResponseDto;
        //        }

        //        userLocationRes.Geohash = geoHash;
        //        userLocationRes.LastModifiedBy = requestDto.ModifiedBy;
        //        userLocationRes.Latitude = requestDto.Latitude;
        //        userLocationRes.Longitude = requestDto.Longitude;

        //        ResponseDto.Message = "Location updated";
        //        ResponseDto.IsSuccess = true;
        //        ResponseDto.Data = null;

        //        _dbContext.DriverLocations.Update(userLocationRes);
        //        await _dbContext.SaveChangesAsync();

        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError($"{nameof(DriverLocationController)}>{nameof(PushLocation)}: {e.Message}");
        //        ResponseDto.IsSuccess = false;
        //        ResponseDto.Data = null;
        //        ResponseDto.Message = e.Message;
        //    }

        //    return ResponseDto;
        //}

    }
}
