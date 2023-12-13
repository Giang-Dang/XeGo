using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Notifications.Functions.Entities
{
    public class ScheduledTask : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string Type { get; set; } = string.Empty!;
        [Required] public int ExecutionUtcYear { get; set; }
        [Required] public int ExecutionUtcMonth { get; set; }
        [Required] public int ExecutionUtcDay { get; set; }
        [Required] public int ExecutionUtcHour { get; set; }
        [Required] public int ExecutionUtcMinute { get; set; }
        [Required] public string? Json { get; set; }
    }
}
