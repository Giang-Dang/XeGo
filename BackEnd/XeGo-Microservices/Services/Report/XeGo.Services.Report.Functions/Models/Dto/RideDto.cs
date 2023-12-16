using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Report.Functions.Models.Dto
{
    class RideDto : BaseEntity
    {
        public int Id { get; set; }
        public string RiderId { get; set; } = string.Empty!;
        public string? DriverId { get; set; }
        public int? DiscountId { get; set; }
        public int? VehicleId { get; set; }
        public int VehicleTypeId { get; set; }
        public string Status { get; set; } = String.Empty!;
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public string StartAddress { get; set; } = String.Empty!;
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
        public string DestinationAddress { get; set; } = String.Empty!;
        public DateTime PickupTime { get; set; }
        public bool IsScheduleRide { get; set; } = false;
        public string? CancelledBy { get; set; }
        public string? CancellationReason { get; set; }
    }
}
