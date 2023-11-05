using Microsoft.AspNetCore.Identity;

namespace XeGo.Services.Auth.API.Entities
{
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
