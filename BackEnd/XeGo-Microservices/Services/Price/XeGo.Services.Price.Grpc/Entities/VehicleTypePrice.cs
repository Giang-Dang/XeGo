using System.ComponentModel.DataAnnotations;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Price.Grpc.Entities
{
    public class VehicleTypePrice : BaseEntity
    {
        [Key] public int VehicleTypeId { get; set; }
        public double PricePerKm { get; set; }
        public double DropCharge { get; set; }
    }
}
