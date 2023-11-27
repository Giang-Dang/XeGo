namespace XeGo.Services.Price.API.Models
{
    public class EditVehicleTypePriceRequestDto
    {
        public int VehicleTypeId { get; set; }
        public double? PricePerKm { get; set; }
        public double? DropCharge { get; set; }
        public string ModifiedBy { get; set; } = string.Empty!;
    }
}
