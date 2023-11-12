using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Driver.API.Entities
{
    [Index(nameof(UserId), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]
    public class DriverInfo : BaseEntity
    {
        [Required][Key] public string UserId { get; set; } = string.Empty!;
        [Required] public string FirstName { get; set; } = string.Empty!;
        [Required] public string LastName { get; set; } = string.Empty!;
        [Required] public string Email { get; set; } = string.Empty!;
        [Required] public string PhoneNumber { get; set; } = string.Empty!;
        [Required] public string Address { get; set; } = string.Empty!;
        [Required] public string District { get; set; } = string.Empty!;
        [Required] public string City { get; set; } = string.Empty!;
    }
}
