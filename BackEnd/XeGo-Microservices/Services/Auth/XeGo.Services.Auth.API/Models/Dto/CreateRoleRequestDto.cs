namespace XeGo.Services.Auth.API.Models.Dto
{
    public class CreateRoleRequestDto
    {
        public string RoleName { get; set; } = string.Empty!;

        public CreateRoleRequestDto(string roleName)
        {
            RoleName = roleName;
        }
    }
}
