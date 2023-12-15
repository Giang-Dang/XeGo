using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Rating.API.Entities
{
    public class UserRating : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public int RideId { get; set; }
        [Required] public string FromUserId { get; set; } = string.Empty!;
        [Required] public string FromUserRole { get; set; } = string.Empty!;

        [Required] public string ToUserId { get; set; } = string.Empty!;
        [Required] public string ToUserRole { get; set; } = string.Empty!;
        [Required] public double Rating { get; set; }
    }
}
