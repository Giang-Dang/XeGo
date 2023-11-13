using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Driver.API.Models
{
    public class CreateVehicleRequestDto
    {
        [Required] public string PlateNumber { get; set; } = string.Empty!;
        [Required] public string Type { get; set; } = string.Empty!;
        [Required] public string DriverId { get; set; } = string.Empty!;
        [Required] public bool IsActive { get; set; } = true;
        public string? ModifiedBy { get; set; }
    }
}
