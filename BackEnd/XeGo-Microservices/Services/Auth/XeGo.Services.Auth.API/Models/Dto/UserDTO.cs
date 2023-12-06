namespace XeGo.Services.Auth.API.Models.Dto
{
    public class UserDto
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
