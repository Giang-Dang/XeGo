using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.File.API.Entities
{
    public class UserFiles : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string UserId { get; set; } = string.Empty!;
        [Required] public string Name { get; set; } = string.Empty!;
        [Required] public string Type { get; set; } = string.Empty!;
        [Required] public string BlobName { get; set; } = string.Empty!;
    }
}
