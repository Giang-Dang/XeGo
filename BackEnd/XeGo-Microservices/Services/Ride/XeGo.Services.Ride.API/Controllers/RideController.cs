using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq.Expressions;
using XeGo.Services.Ride.API.Models.Dto;
using XeGo.Services.Ride.API.Repository.IRepository;
using XeGo.Services.Ride.API.Secrets;
using XeGo.Shared.GrpcConsumer.Services;
using XeGo.Shared.Lib.Helpers;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Ride.API.Controllers
{
    [Route("api/rides")]
    [ApiController]
    public class RideController(
        IRideRepository rideRepo,
        VehicleTypePriceGrpcService vehicleTypePriceGrpcService,
        PriceGrpcService priceGrpcService,
        ILogger<RideController> logger) : ControllerBase
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
            DateTime? pickupTimeStart,
            DateTime? pickupTimeEnd,
            bool? isScheduleRide,
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
                    (couponId == null || r.DiscountId == couponId) &&
                    (status == null || r.Status == status) &&
                    (startAddress == null || r.StartAddress == startAddress) &&
                    (destinationAddress == null || r.DestinationAddress == destinationAddress) &&
                    (pickupTimeStart == null || r.PickupTime >= pickupTimeStart) &&
                    (pickupTimeEnd == null || r.PickupTime <= pickupTimeEnd) &&
                    (isScheduleRide == null || r.IsScheduleRide == isScheduleRide.Value) &&
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

        [HttpGet("directions")]
        public async Task<ResponseDto> GetDirectionApi(
            double startLat,
            double startLng,
            double endLat,
            double endLng)
        {
            var url = $"https://maps.googleapis.com/maps/api/directions/json?origin={startLat},{startLng}&destination={endLat},{endLng}&key={ApiKey.GoogleMapApiKey}";

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var stringResponse = await response.Content.ReadAsStringAsync();

                    ResponseDto.IsSuccess = true;
                    ResponseDto.Data = stringResponse;
                }
                catch (HttpRequestException httpRequestException)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = httpRequestException.Message;
                }
            }
            return ResponseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> CreateRide(CreateRideRequestDto requestDto)
        {
            logger.LogInformation($"Executing {nameof(RideController)}>{nameof(CreateRide)} from userId:{requestDto.ModifiedBy}...");

            try
            {
                DateTime pickUpTime;
                string format = "yyyy-MM-ddTHH:mm:ss.fffZ";
                DateTimeStyles styles = DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal;

                if (!DateTime.TryParse(requestDto.PickupTime, out pickUpTime))
                {
                    if (!DateTime.TryParseExact(requestDto.PickupTime, format, CultureInfo.InvariantCulture, styles, out pickUpTime))
                    {
                        logger.LogError($"{nameof(RideController)}>{nameof(CreateRide)} : Can't convert {requestDto.PickupTime}");
                    }
                }
                logger.LogInformation($"{nameof(RideController)}>{nameof(CreateRide)} : {requestDto.PickupTime}");

                logger.LogInformation($"{nameof(RideController)}>{nameof(CreateRide)} : {pickUpTime}");

                var createDto = new Entities.Ride()
                {
                    RiderId = requestDto.RiderId,
                    DriverId = requestDto.DriverId,
                    DiscountId = requestDto.DiscountId,
                    Status = requestDto.Status,
                    VehicleId = requestDto.VehicleId,
                    VehicleTypeId = requestDto.VehicleTypeId,
                    StartLatitude = requestDto.StartLatitude,
                    StartLongitude = requestDto.StartLongitude,
                    StartAddress = requestDto.StartAddress,
                    DestinationLatitude = requestDto.DestinationLatitude,
                    DestinationLongitude = requestDto.DestinationLongitude,
                    DestinationAddress = requestDto.DestinationAddress,
                    PickupTime = pickUpTime,
                    IsScheduleRide = requestDto.IsScheduleRide,
                    CreatedBy = requestDto.ModifiedBy,
                    LastModifiedBy = requestDto.ModifiedBy,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow
                };

                ResponseDto.Data = await rideRepo.CreateAsync(createDto);

                var createdPriceResponse = await priceGrpcService.CreatePrice(createDto.Id, createDto.DiscountId, requestDto.VehicleTypeId,
                   requestDto.DistanceInMeters, requestDto.ModifiedBy);

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

                cRide.RiderId = requestDto.RiderId ?? cRide.RiderId;
                cRide.DriverId = requestDto.DriverId ?? cRide.DriverId;
                cRide.DiscountId = requestDto.CouponId ?? cRide.DiscountId;
                cRide.Status = requestDto.Status ?? cRide.Status;
                cRide.VehicleId = requestDto.VehicleId ?? cRide.VehicleId;
                cRide.StartLatitude = requestDto.StartLatitude ?? cRide.StartLatitude;
                cRide.StartLongitude = requestDto.StartLongitude ?? cRide.StartLongitude;
                cRide.StartAddress = requestDto.StartAddress ?? cRide.StartAddress;
                cRide.DestinationLatitude = requestDto.DestinationLatitude ?? cRide.DestinationLatitude;
                cRide.DestinationLongitude = requestDto.DestinationLongitude ?? cRide.DestinationLongitude;
                cRide.DestinationAddress = requestDto.DestinationAddress ?? cRide.DestinationAddress;
                cRide.CancellationReason = requestDto.CancellationReason ?? cRide.CancellationReason;
                cRide.CancelledBy = requestDto.CancelledBy ?? cRide.CancelledBy;
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

        [HttpGet("estimated-price")]
        public async Task<ResponseDto> GetEstimatedPrice(
            int vehicleTypeId,
            double distanceInMeters,
            int? discountId)
        {
            logger.LogInformation($"Executing {nameof(RideController)}>{nameof(GetEstimatedPrice)}:...");

            try
            {
                var vehicleTypePriceResponse = await vehicleTypePriceGrpcService.GetVehicleTypePriceById(vehicleTypeId);
                var cVehicleTypePrice =
                    JsonConvert.DeserializeObject<VehicleTypePriceDto>(vehicleTypePriceResponse.Data);

                if (!vehicleTypePriceResponse.IsSuccess || cVehicleTypePrice == null)
                {
                    logger.LogError($"{nameof(RideController)}>{nameof(GetEstimatedPrice)}: Not Found!");
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = vehicleTypePriceResponse.Message;
                    return ResponseDto;
                }

                var totalPrice = distanceInMeters < 500
                    ? cVehicleTypePrice.DropCharge
                    : Math.Round(distanceInMeters / 1000 * cVehicleTypePrice.PricePerKm, 2);

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = totalPrice;
            }
            catch (Exception e)
            {
                logger.LogError($"{nameof(RideController)}>{nameof(GetEstimatedPrice)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

    }

}
