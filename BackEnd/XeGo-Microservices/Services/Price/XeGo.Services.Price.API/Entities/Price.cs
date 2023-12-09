using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Price.API.Entities
{
    public class Price : BaseEntity
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.None)] public int RideId { get; set; }
        public int? DiscountId { get; set; }
        [ForeignKey($"{nameof(DiscountId)}")] public virtual Discount? Discount { get; set; }
        public int VehicleTypeId { get; set; }

        [ForeignKey($"{nameof(VehicleTypeId)}")]
        public virtual VehicleTypePrice VehicleTypePrice { get; set; } = null!;
        public double DistanceInMeters { get; set; }

        public double TotalPrice { get; set; }
    }

}
