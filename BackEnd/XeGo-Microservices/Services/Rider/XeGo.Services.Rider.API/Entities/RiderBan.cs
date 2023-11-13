using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Rider.API.Entities
{
    public class RiderBan : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string RiderId { get; set; } = string.Empty!;
        [Required] public RiderInfo RiderInfo { get; set; } = null!;
        [Required] public string Reason { get; set; } = string.Empty!;
        [Required] public DateTime StartTime { get; set; }
        [Required] public DateTime EndTime { get; set; }
        [Required] public bool IsActive { get; set; } = true;

    }
}
