using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Driver.API.Models
{
    public class CreateDriverInfoRequestDto
    {
        [Required] public string DriverId { get; set; } = string.Empty!;

        [Required] public string FirstName { get; set; } = string.Empty!;
        [Required] public string LastName { get; set; } = string.Empty!;
        [Required] public string Email { get; set; } = string.Empty!;
        [Required] public string PhoneNumber { get; set; } = string.Empty!;
        [Required] public string Address { get; set; } = string.Empty!;
        [Required] public string District { get; set; } = string.Empty!;
        [Required] public string City { get; set; } = string.Empty!;
    }
}
