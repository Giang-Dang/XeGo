using System.ComponentModel.DataAnnotations;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Notifications.Functions.Entities
{
    public class UserConnectionInfo : BaseEntity
    {
        [Key] public string UserId { get; set; } = string.Empty!;
        public string? FcmToken { get; set; }
    }
}
