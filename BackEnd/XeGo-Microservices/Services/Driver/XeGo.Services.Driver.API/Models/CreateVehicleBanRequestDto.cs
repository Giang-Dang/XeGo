using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Driver.API.Models
{
    public class CreateVehicleBanRequestDto
    {
        [Required] public string Reason { get; set; } = string.Empty!;
        [Required] public DateTime StartTime { get; set; } = DateTime.UtcNow;
        [Required] public DateTime EndTime { get; set; }
        [Required] public bool IsActive { get; set; } = true;
        public string? ModifiedBy { get; set; }
    }
}
