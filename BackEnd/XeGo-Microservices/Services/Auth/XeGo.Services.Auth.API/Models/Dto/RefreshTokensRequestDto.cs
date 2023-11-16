using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Auth.API.Models.Dto
{
    public class RefreshTokensRequestDto
    {
        [Required] public string RefreshToken { get; set; } = string.Empty!;
        [Required] public string UserId { get; set; } = string.Empty!;
        [Required] public string FromApp { get; set; } = string.Empty!;
    }
}
