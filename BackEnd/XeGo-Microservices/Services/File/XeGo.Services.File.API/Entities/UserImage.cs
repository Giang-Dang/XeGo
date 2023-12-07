using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.File.API.Entities
{
    [Index(nameof(UserId), IsUnique = false)]

    public class UserImage : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string UserId { get; set; } = string.Empty!;
        [Required] public string ImageName { get; set; } = string.Empty!;
        [Required] public string ImageType { get; set; } = string.Empty!;
        [Required] public string ImageSize { get; set; } = string.Empty!;
        [Required] public string BlobName { get; set; } = string.Empty!;

    }
}
