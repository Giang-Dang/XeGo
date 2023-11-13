using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Rider.API.Models
{
    public class EditRiderBanRequestDto
    {
        [Required] public int Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Reason { get; set; }
        public bool? IsActive { get; set; }
        public string? ModifiedBy { get; set; }
    }

}
