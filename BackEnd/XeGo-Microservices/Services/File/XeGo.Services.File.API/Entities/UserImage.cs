using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeGo.Services.Media.API.Entities
{
    [Index(nameof(UserId), IsUnique = true)]

    public class UserImage
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
