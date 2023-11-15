using XeGo.Services.Auth.API.Models.Dto;

namespace XeGo.Services.Auth.API.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRoleAsync(string email, string roleName);
        Task<bool> RemoveRoleAsync(string userId, string roleName);
        bool CreateRole(string roleName);
        bool RemoveRole(string roleName);
    }
}
