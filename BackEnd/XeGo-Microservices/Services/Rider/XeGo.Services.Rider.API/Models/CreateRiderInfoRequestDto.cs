using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Rider.API.Models
{
    public class CreateRiderInfoRequestDto
    {
        [Required] public string UserId { get; set; } = string.Empty!;
        [Required] public string FirstName { get; set; } = string.Empty!;
        [Required] public string LastName { get; set; } = string.Empty!;
        [Required] public string Email { get; set; } = string.Empty!;
        [Required] public string PhoneNumber { get; set; } = string.Empty!;
        [Required] public string Address { get; set; } = string.Empty!;
        [Required] public string District { get; set; } = string.Empty!;
        [Required] public string City { get; set; } = string.Empty!;
        public string? ModifiedBy { get; set; }
    }

}
