using XeGo.Services.Auth.AuthAPI.Models;

namespace XeGo.Services.Auth.AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
