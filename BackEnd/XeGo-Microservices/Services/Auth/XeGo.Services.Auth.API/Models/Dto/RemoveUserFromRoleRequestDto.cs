using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Auth.API.Models.Dto
{
    public class RemoveUserFromRoleRequestDto
    {
        [Required] public string UserId { get; set; } = string.Empty!;
        [Required] public string RoleName { get; set; } = string.Empty!;
    }
}
