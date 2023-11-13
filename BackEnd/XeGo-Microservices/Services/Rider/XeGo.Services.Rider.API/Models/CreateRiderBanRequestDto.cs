using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Rider.API.Models
{
    public class CreateRiderBanRequestDto
    {
        [Required] public string RiderId { get; set; } = String.Empty!;
        [Required] public DateTime StartTime { get; set; }
        [Required] public DateTime EndTime { get; set; }
        [Required] public string Reason { get; set; } = String.Empty!;
        public string? ModifiedBy { get; set; }
    }

}
