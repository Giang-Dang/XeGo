using System.ComponentModel.DataAnnotations;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Location.Grpc.Entities
{
    public class VehicleDto : BaseEntity
    {
        public int Id { get; set; }
        [Required] public string PlateNumber { get; set; } = string.Empty!;
        [Required] public int TypeId { get; set; }
        [Required] public bool IsAssigned { get; set; } = false;

        [Required] public bool IsActive { get; set; } = true;
    }
}
