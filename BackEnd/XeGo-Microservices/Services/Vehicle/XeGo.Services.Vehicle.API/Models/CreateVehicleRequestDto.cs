using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Vehicle.API.Models
{
    public class CreateVehicleRequestDto
    {
        [Required] public string PlateNumber { get; set; } = string.Empty!;
        [Required] public int TypeId { get; set; }
        [Required] public bool IsActive { get; set; } = true;
        public string? ModifiedBy { get; set; }
    }
}
