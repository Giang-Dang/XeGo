namespace XeGo.Services.Auth.API.Models.Dto
{
    public class LoginResponseDto
    {
        public UserDto? User { get; set; }
        public TokenDto Tokens { get; set; } = new();
        public List<String> Roles { get; set; } = new();
    }
}
