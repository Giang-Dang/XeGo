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
        [Required] public int TypeId { get; set; }
        [ForeignKey($"{nameof(TypeId)}")] public virtual VehicleType VehicleType { get; set; } = null!;
        public string? DriverId { get; set; }
        [Required] public bool IsActive { get; set; } = true;
    }
}
