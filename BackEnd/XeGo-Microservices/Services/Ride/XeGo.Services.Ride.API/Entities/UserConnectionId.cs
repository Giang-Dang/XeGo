using System.ComponentModel.DataAnnotations;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Ride.API.Entities
{
    public class UserConnectionId : BaseEntity
    {
        [Key] public string UserId { get; set; } = string.Empty!;
        [Required] public string ConnectionId { get; set; } = String.Empty!;
    }
}
