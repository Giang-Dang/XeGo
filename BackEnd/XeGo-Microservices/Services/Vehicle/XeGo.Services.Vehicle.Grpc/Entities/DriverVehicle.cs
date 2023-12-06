using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Vehicle.Grpc.Entities
{
    public class DriverVehicle : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        [Required] public string DriverId { get; set; } = string.Empty!;
        [ForeignKey($"{nameof(DriverId)}")] public Driver Driver { get; set; } = null!;
        [Required] public int VehicleId { get; set; }
        [ForeignKey($"{nameof(VehicleId)}")] public Vehicle Vehicle { get; set; } = null!;
        [Required] public bool IsDeleted { get; set; } = false;
    }
}
