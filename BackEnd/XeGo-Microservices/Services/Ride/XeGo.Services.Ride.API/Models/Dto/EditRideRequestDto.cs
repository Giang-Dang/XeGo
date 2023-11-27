using System.ComponentModel.DataAnnotations;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Ride.API.Models.Dto
{
    public class EditRideRequestDto : BaseEntity
    {
        [Required] public int Id { get; set; }
        public string? RiderId { get; set; }
        public string? DriverId { get; set; }
        public int? CouponId { get; set; }
        public int? VehicleId { get; set; }
        public string? Status { get; set; }
        public double? StartLatitude { get; set; }
        public double? StartLongitude { get; set; }
        public string? StartAddress { get; set; }
        public double? DestinationLatitude { get; set; }
        public double? DestinationLongitude { get; set; }
        public string? DestinationAddress { get; set; }
        public DateTime? PickupTime { get; set; }
        public bool? IsScheduleRide { get; set; }
        public string? CancelledBy { get; set; }
        public string? CancellationReason { get; set; }
        [Required] public string ModifiedBy { get; set; } = String.Empty!;
    }
}
