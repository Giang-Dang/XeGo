using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Auth.API.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required] public string FirstName { get; set; } = string.Empty!;
        [Required] public string LastName { get; set; } = string.Empty!;
        [Required] public string Address { get; set; } = string.Empty!;
    }
}
