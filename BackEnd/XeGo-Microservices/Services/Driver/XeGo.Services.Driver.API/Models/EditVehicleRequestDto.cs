using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Driver.API.Models
{
    public class EditVehicleRequestDto
    {
        public int Id { get; set; }
        [Required] public string PlateNumber { get; set; } = string.Empty!;
        [Required] public string Type { get; set; } = string.Empty!;
        [Required] public string DriverId { get; set; } = string.Empty!;
        [Required] public bool IsActive { get; set; } = true;
        [Required] public string ModifiedBy { get; set; } = string.Empty!;
    }
}
