using XeGo.Services.Auth.API.Models.Dto;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Auth.API.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto> Register(RegistrationRequestDto requestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<string?> RefreshToken(string refreshToken, string userId, string loginApp);
        Task<bool> AssignRoleAsync(string email, string roleName);
        Task<bool> RemoveRoleAsync(string userId, string roleName);
        bool CreateRole(string roleName);
        bool RemoveRole(string roleName);
    }
}
