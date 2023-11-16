using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Vehicle.API.Models
{
    public class EditVehicleBanRequestDto
    {
        [Required] public string Reason { get; set; } = string.Empty!;
        [Required] public DateTime StartTime { get; set; } = DateTime.UtcNow;
        [Required] public DateTime EndTime { get; set; }
        [Required] public bool IsActive { get; set; } = true;
        public string? ModifiedBy { get; set; }
    }
}
