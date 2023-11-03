using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeGo.Services.CodeValue.API.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowId { get; set; }
        public string CreatedBy { get; set; } = string.Empty!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string LastModifiedBy { get; set; } = string.Empty!;
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
