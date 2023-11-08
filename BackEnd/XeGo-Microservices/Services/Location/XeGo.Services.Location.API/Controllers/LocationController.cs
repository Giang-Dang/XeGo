using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XeGo.Services.Location.API.Data;
using XeGo.Services.Location.API.Entities;
using XeGo.Services.Location.API.Models.Dto;
using XeGo.Services.Location.API.Services.IServices;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Location.API.Controllers
{
    [Route("api/location")]
    [ApiController]
    public class LocationController
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LocationController> _logger;
        private readonly IGeoHashService _geoHashService;
        private ResponseDto ResponseDto { get; set; }

        public LocationController(AppDbContext dbContext, IMapper mapper, ILogger<LocationController> logger, IGeoHashService geoHashService)
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
                var userLocationRes = await _dbContext.UserLocations.FirstOrDefaultAsync(e => e.UserId == userId);
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
                _logger.LogError($"{nameof(LocationController)}>{nameof(GetLocationByUserId)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> PushLocation([FromBody] UserLocationRequestDto requestDto)
        {
            try
            {
                if (!_geoHashService.IsValidCoordinates(requestDto.Latitude, requestDto.Longitude))
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = "Invalid Coordinate!";
                    _logger.LogInformation($"{nameof(LocationController)}>{nameof(PushLocation)}: Invalid Coordinate!");
                    return ResponseDto;
                }

                var geoHash = _geoHashService.Geohash(requestDto.Latitude, requestDto.Longitude, 500.0);
                var userLocationRes =
                    await _dbContext.UserLocations.FirstOrDefaultAsync(e => e.UserId == requestDto.UserId);

                if (userLocationRes == null)
                {
                    UserLocation createEntity = new();
                    _mapper.Map(requestDto, createEntity);

                    createEntity.Geohash = geoHash;
                    createEntity.CreatedBy = requestDto.UserId;
                    createEntity.LastModifiedBy = requestDto.UserId;

                    await _dbContext.UserLocations.AddAsync(createEntity);
                    await _dbContext.SaveChangesAsync();

                    ResponseDto.Message = "Location created";
                    ResponseDto.IsSuccess = true;
                    ResponseDto.Data = null;

                    return ResponseDto;
                }

                userLocationRes.Geohash = geoHash;
                userLocationRes.CreatedBy = userLocationRes.UserId;
                userLocationRes.LastModifiedBy = userLocationRes.UserId;
                userLocationRes.Latitude = requestDto.Latitude;
                userLocationRes.Longitude = requestDto.Longitude;

                _dbContext.UserLocations.Update(userLocationRes);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(LocationController)}>{nameof(PushLocation)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost("/nearby-drivers")]
        public async Task<List<string>> FindNearbyDrivers([FromBody] UserLocationRequestDto requestDto)
        {
            var neighborsGeohash = _geoHashService.GetNeighbors(requestDto.Latitude, requestDto.Longitude, 500, 1000);

            var usersList = new List<string>();
            foreach (var geohash in neighborsGeohash)
            {
                var users = await _dbContext.UserLocations.AsNoTracking().Where(u => u.Geohash == geohash).Select(u => u.UserId).ToListAsync();
                usersList.AddRange(users);
            }

            return usersList;
        }

    }
}
