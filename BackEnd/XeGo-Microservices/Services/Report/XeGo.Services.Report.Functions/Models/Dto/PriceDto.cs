using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Report.Functions.Models.Dto
{
    public class PriceDto : BaseEntity
    {
        public int RideId { get; set; }
        public int? DiscountId { get; set; }
        public int VehicleTypeId { get; set; }
        public double DistanceInMeters { get; set; }
        public double TotalPrice { get; set; }
    }
}
