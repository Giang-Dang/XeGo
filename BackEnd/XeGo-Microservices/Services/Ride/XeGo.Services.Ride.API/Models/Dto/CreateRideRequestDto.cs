using System.ComponentModel.DataAnnotations;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Ride.API.Models.Dto
{
    public class CreateRideRequestDto
    {
        [Required] public string RiderId { get; set; } = string.Empty!;
        public string? DriverId { get; set; }
        public int? DiscountId { get; set; }
        public int? VehicleId { get; set; }
        [Required] public int VehicleTypeId { get; set; }
        [Required] public string Status { get; set; } = RideStatusConstants.FindingDriver;
        [Required] public double StartLatitude { get; set; }
        [Required] public double StartLongitude { get; set; }
        [Required] public string StartAddress { get; set; } = String.Empty!;
        [Required] public double DestinationLatitude { get; set; }
        [Required] public double DestinationLongitude { get; set; }
        [Required] public string DestinationAddress { get; set; } = String.Empty!;
        [Required] public double DistanceInMeters { get; set; }
        [Required] public string PickupTime { get; set; } = String.Empty!;
        [Required] public bool IsScheduleRide { get; set; }
        public string? CancelledBy { get; set; }
        public string? CancellationReason { get; set; }
        [Required] public string ModifiedBy { get; set; } = String.Empty!;
    }
}
