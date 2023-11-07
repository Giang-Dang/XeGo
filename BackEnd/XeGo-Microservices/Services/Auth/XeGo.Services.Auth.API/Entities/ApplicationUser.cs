using Microsoft.AspNetCore.Identity;

namespace XeGo.Services.Auth.API.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty!;

    }
}
