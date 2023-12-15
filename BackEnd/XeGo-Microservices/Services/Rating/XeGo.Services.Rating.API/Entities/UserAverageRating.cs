using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Rating.API.Entities
{
    public class UserAverageRating : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string UserId { get; set; } = string.Empty!;
        [Required] public string UserRole { get; set; } = string.Empty!;
        [Required] public double AverageRating { get; set; }
    }
}
