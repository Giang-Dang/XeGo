﻿using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using XeGo.Services.Ride.API.Models.Dto;
using XeGo.Services.Ride.API.Repository.IRepository;
using XeGo.Shared.Lib.Constants;
using XeGo.Shared.Lib.Helpers;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Ride.API.Controllers
{
    [Route("api/rides")]
    [ApiController]
    public class RideController(IRideRepository rideRepo, ILogger<RideController> logger) : ControllerBase
    {
        private ResponseDto ResponseDto { get; set; } = new();

        [HttpGet]
        public async Task<ResponseDto> GetAllRides(
            int? id,
            string? riderId,
            string? driverId,
            int? couponId,
            string? status,
            string? startAddress,
            string? destinationAddress,
            string? cancelledBy,
            string? cancellationReason,
            string? createdBy,
            DateTime? createdStartDate,
            DateTime? createdEndDate,
            string? lastModifiedBy,
            DateTime? lastModifiedStartDate,
            DateTime? lastModifiedEndDate,
            string? searchString,
            int pageNumber = 0,
            int pageSize = 0)
        {
            logger.LogInformation($"Executing {nameof(RideController)}>{nameof(GetAllRides)}...");

            try
            {
                Expression<Func<Entities.Ride, bool>> filters = r =>
                    (id == null || r.Id == id) &&
                    (riderId == null || r.RiderId == riderId) &&
                    (driverId == null || r.DriverId == driverId) &&
                    (couponId == null || r.CouponId == couponId) &&
                    (status == null || r.Status == status) &&
                    (startAddress == null || r.StartAddress == startAddress) &&
                    (destinationAddress == null || r.DestinationAddress == destinationAddress) &&
                    (cancelledBy == null || r.CancelledBy == cancelledBy) &&
                    (cancellationReason == null || r.CancellationReason == cancellationReason) &&
                    (createdBy == null || r.CreatedBy == createdBy) &&
                    (createdStartDate == null || r.CreatedDate >= createdStartDate) &&
                    (createdEndDate == null || r.CreatedDate <= createdEndDate) &&
                    (lastModifiedBy == null || r.LastModifiedBy == lastModifiedBy) &&
                    (lastModifiedStartDate == null || r.LastModifiedDate >= lastModifiedStartDate) &&
                    (lastModifiedEndDate == null || r.LastModifiedDate <= lastModifiedEndDate);

                if (!string.IsNullOrEmpty(searchString))
                {
                    Expression<Func<Entities.Ride, bool>> searchFilter = r =>
                        ((r.RiderId.ToLower().Contains(searchString.ToLower())) ||
                         (r.DriverId != null && r.DriverId.ToLower().Contains(searchString.ToLower())) ||
                         (r.Status.ToLower().Contains(searchString.ToLower())) ||
                         (r.StartAddress.ToLower().Contains(searchString.ToLower())) ||
                         (r.DestinationAddress.ToLower().Contains(searchString.ToLower())) ||
                         (r.CancelledBy != null && r.CancelledBy.ToLower().Contains(searchString.ToLower())) ||
                         (r.CancellationReason != null &&
                          r.CancellationReason.ToLower().Contains(searchString.ToLower())) ||
                         (r.CreatedBy.ToLower().Contains(searchString.ToLower())) ||
                         (r.LastModifiedBy.ToLower().Contains(searchString.ToLower())));

                    filters = filters.AndAlso(searchFilter);
                }

                var rides = await rideRepo.GetAllAsync(
                    filter: filters,
                    pageSize: pageSize,
                    pageNumber: pageNumber);

                logger.LogInformation($"Get rides : {rides.Count()} Found!");
                ResponseDto.IsSuccess = true;
                ResponseDto.Data = rides;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(RideController)}>{nameof(GetAllRides)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }


        [HttpPost]
        public async Task<ResponseDto> CreateRide(CreateRideRequestDto requestDto)
        {
            logger.LogInformation($"Executing {nameof(RideController)}>{nameof(CreateRide)} from userId:{requestDto.ModifiedBy}...");

            try
            {
                var createDto = new Entities.Ride()
                {
                    RiderId = requestDto.RiderId,
                    DriverId = requestDto.DriverId,
                    CouponId = requestDto.CouponId,
                    Status = requestDto.Status ?? RideStatusConstants.FindingDriver,
                    VehicleId = requestDto.VehicleId,
                    StartLatitude = requestDto.StartLatitude,
                    StartLongitude = requestDto.StartLongitude,
                    StartAddress = requestDto.StartAddress,
                    DestinationLatitude = requestDto.DestinationLatitude,
                    DestinationLongitude = requestDto.DestinationLongitude,
                    DestinationAddress = requestDto.DestinationAddress,
                    CreatedBy = requestDto.ModifiedBy,
                    LastModifiedBy = requestDto.ModifiedBy,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow
                };

                ResponseDto.Data = await rideRepo.CreateAsync(createDto);
                ResponseDto.IsSuccess = true;
                logger.LogInformation($"Ride is created!");
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(RideController)}>{nameof(CreateRide)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPut]
        public async Task<ResponseDto> EditRide(EditRideRequestDto requestDto)
        {
            logger.LogInformation($"Executing {nameof(RideController)}>{nameof(EditRide)} id:{requestDto.Id}...");

            try
            {
                var cRide = await rideRepo.GetAsync(r => r.Id == requestDto.Id);
                if (cRide == null)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = "Ride not found!";
                    return ResponseDto;
                }

                cRide.RiderId = requestDto.RiderId;
                cRide.DriverId = requestDto.DriverId;
                cRide.CouponId = requestDto.CouponId;
                cRide.Status = requestDto.Status;
                cRide.VehicleId = requestDto.VehicleId;
                cRide.StartLatitude = requestDto.StartLatitude;
                cRide.StartLongitude = requestDto.StartLongitude;
                cRide.StartAddress = requestDto.StartAddress;
                cRide.DestinationLatitude = requestDto.DestinationLatitude;
                cRide.DestinationLongitude = requestDto.DestinationLongitude;
                cRide.DestinationAddress = requestDto.DestinationAddress;
                cRide.CancellationReason = requestDto.CancellationReason;
                cRide.CancelledBy = requestDto.CancelledBy;
                cRide.LastModifiedBy = requestDto.ModifiedBy;
                cRide.LastModifiedDate = DateTime.UtcNow;

                ResponseDto.Data = await rideRepo.UpdateAsync(cRide);
                ResponseDto.IsSuccess = true;
                logger.LogInformation($"Ride is edited!");
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(RideController)}>{nameof(EditRide)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }
    }

}