using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Auth.API.Models.Dto
{
    public class RegistrationRequestDto
    {
        public string? Email { get; set; }
        [Required] public string UserName { get; set; } = string.Empty!;
        [Required] public string PhoneNumber { get; set; } = string.Empty!;
        [Required] public string Password { get; set; } = string.Empty!;
        [Required] public string FirstName { get; set; } = string.Empty!;
        [Required] public string LastName { get; set; } = string.Empty!;
        [Required] public string Address { get; set; } = string.Empty!;
        [Required] public string Role { get; set; } = string.Empty!;

    }
}
