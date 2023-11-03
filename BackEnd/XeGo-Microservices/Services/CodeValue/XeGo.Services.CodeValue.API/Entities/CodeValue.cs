using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace XeGo.Services.CodeValue.API.Entities
{
    [Index(nameof(Name), IsUnique = false)]
    public class CodeValue : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty!;
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
        public int? SortOrder { get; set; } = null;
        [Required]
        public DateTime EffectiveStartDate { get; set; }
        [Required]
        public DateTime EffectiveEndDate { get; set; }
        [MaxLength(100)]
        public string? Value1 { get; set; }
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
