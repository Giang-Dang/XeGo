using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Vehicle.API.Models
{
    public class CreateVehicleTypeRequestDto
    {
        [Required] public string Name { get; set; } = string.Empty!;
        [Required] public string ModifiedBy { get; set; } = string.Empty!;
        [Required] public bool IsActive { get; set; } = true;
    }
}
