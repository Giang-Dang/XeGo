namespace XeGo.Services.Vehicle.API.Models
{
    public class EditVehicleRequestDto
    {
        public int Id { get; set; }
        public string? PlateNumber { get; set; }
        public int? TypeId { get; set; }
        public string? DriverId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAssigned { get; set; }
        public string ModifiedBy { get; set; } = string.Empty!;
    }
}
