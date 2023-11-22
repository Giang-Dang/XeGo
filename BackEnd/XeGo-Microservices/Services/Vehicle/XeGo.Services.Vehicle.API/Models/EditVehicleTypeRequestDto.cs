using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Vehicle.API.Models
{
    public class EditVehicleTypeRequestDto
    {
        [Required] public int Id { get; set; }
        public string? Name { get; set; }
        [Required] public string ModifiedBy { get; set; } = string.Empty!;
        public bool? IsActive { get; set; }
    }
}
