using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Vehicle.API.Entities
{
    public class Vehicle : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string PlateNumber { get; set; } = string.Empty!;
        [Required] public string Type { get; set; } = string.Empty!;
        public string? CurrentDriverId { get; set; }
        [Required] public bool IsActive { get; set; } = true;
    }
}
