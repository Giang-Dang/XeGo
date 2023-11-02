using Microsoft.AspNetCore.Identity;

namespace XeGo.Services.Auth.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty!;

    }
}
