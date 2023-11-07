namespace XeGo.Services.Auth.API.Models.Dto
{
    public class TokenDto
    {
        public string? RefreshToken { get; set; }
        public string? AccessToken { get; set; }
    }
}
