using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Auth.API.Models.Dto
{
    public class RefreshTokenRequestDto
    {
        [Required] public string RefreshToken = string.Empty!;
        [Required] public string UserId = string.Empty!;
        [Required] public string FromApp = string.Empty!;
    }
}
