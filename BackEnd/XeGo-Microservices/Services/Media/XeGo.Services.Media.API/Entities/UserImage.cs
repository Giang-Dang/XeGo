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

        [Required][MaxLength(450)] public string UserId { get; set; } = string.Empty!;
        [Required] public int ImageTypeCode { get; set; }
        [Required] public int ImageSizeCode { get; set; }
        [Required] public string AbsoluteUri { get; set; } = string.Empty!;

    }
}
