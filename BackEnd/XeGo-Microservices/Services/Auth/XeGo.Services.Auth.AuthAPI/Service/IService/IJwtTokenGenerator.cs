using XeGo.Services.Auth.API.Entities;

namespace XeGo.Services.Auth.API.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
