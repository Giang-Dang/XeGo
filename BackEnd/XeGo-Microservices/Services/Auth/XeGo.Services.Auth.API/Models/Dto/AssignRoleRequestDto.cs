using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.Auth.API.Models.Dto
{
    public class AssignRoleRequestDto
    {
        [Required] public string UserId { get; set; } = string.Empty!;
        [Required] public string RoleName { get; set; } = string.Empty!;
    }
}
