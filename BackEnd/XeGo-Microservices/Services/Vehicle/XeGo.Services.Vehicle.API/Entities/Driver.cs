using System.ComponentModel.DataAnnotations;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Vehicle.API.Entities
{
    public class Driver : BaseEntity
    {
        [Key] public string UserId { get; set; } = string.Empty!;
        [Required] public string UserName { get; set; } = string.Empty!;
        [Required] public string FirstName { get; set; } = string.Empty!;
        [Required] public string LastName { get; set; } = string.Empty!;
        [Required] public string PhoneNumber { get; set; } = string.Empty!;
        [Required] public string Email { get; set; } = string.Empty!;
        [Required] public string Address { get; set; } = string.Empty!;
        [Required] public bool IsAssigned { get; set; } = false;
    }
}
