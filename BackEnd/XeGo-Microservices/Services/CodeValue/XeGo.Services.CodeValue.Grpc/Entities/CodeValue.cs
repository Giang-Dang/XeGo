using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.CodeValue.Grpc.Entities
{
    [Index(nameof(Name), IsUnique = false)]
    [Index(nameof(Value1), IsUnique = false)]
    public class CodeValue : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty!;
        public int? SortOrder { get; set; } = null;
        [Required]
        public DateTime EffectiveStartDate { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime EffectiveEndDate { get; set; } = DateTime.MaxValue;

        [MaxLength(100)]
        [Required]
        public string Value1 { get; set; } = string.Empty!;
        [MaxLength(100)]
        public string? Value2 { get; set; }
        [MaxLength(100)]
        public string? Value3 { get; set; }
        [MaxLength(100)]
        public string? Value4 { get; set; }
        [MaxLength(100)]
        public string? Value5 { get; set; }
        [MaxLength(100)]
        public string? Value6 { get; set; }
        [MaxLength(100)]
        public string? Value7 { get; set; }
        [MaxLength(100)]
        public string? Value8 { get; set; }
        [MaxLength(100)]
        public string? Value9 { get; set; }
        [MaxLength(100)]
        public string? Value10 { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
