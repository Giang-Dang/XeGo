namespace XeGo.Services.Auth.AuthAPI.Models.Dto
{
    public class LoginResponseDto
    {
        public UserDto? User { get; set; }
        public string? Token { get; set; }
    }
}
