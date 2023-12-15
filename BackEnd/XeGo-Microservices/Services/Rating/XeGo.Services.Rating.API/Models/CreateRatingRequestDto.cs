using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Rating.API.Models
{
    public class CreateRatingRequestDto
    {
        public int Id { get; set; }
        [Required] public int RideId { get; set; }
        [Required] public string FromUserId { get; set; } = string.Empty!;
        [Required] public string FromUserRole { get; set; } = string.Empty!;

        [Required] public string ToUserId { get; set; } = string.Empty!;
        [Required] public string ToUserRole { get; set; } = string.Empty!;
        [Required] public double Rating { get; set; }
        [Required] public string CreatedBy { get; set; } = string.Empty!;
    }
}
