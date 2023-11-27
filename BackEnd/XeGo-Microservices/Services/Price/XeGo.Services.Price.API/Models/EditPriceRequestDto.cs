namespace XeGo.Services.Price.API.Models
{
    public class EditPriceRequestDto
    {
        public int RideId { get; set; }
        public int? DiscountId { get; set; }
        public int? VehicleTypeId { get; set; }
        public double? DistanceInMeters { get; set; }
        public string ModifiedBy { get; set; } = string.Empty!;
    }
}
