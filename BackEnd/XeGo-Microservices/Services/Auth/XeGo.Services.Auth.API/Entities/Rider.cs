using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Constants;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Auth.API.Entities
{
    public class Rider : BaseEntity
    {
        [Key] public string RiderId { get; set; } = string.Empty!;
        [ForeignKey($"{nameof(RiderId)}")] public virtual ApplicationUser User { get; set; } = null!;
        [Required] public string Type { get; set; } = RiderTypeConstants.Normal;
    }
}
