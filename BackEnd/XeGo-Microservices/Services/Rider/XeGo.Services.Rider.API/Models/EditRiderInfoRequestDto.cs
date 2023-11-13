using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Rider.API.Models
{
    public class EditRiderInfoRequestDto
    {
        [Required] public string UserId { get; set; } = string.Empty!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? ModifiedBy { get; set; }
    }

}
