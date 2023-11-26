using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Price.API.Entities
{
    public class Price : BaseEntity
    {
        [Key] public int RideId { get; set; }
        public int? DiscountId { get; set; }
        [ForeignKey($"{nameof(DiscountId)}")] public virtual Discount? Discount { get; set; }
        public int VehicleTypeId { get; set; }

        [ForeignKey($"{nameof(VehicleTypeId)}")]
        public virtual VehicleTypePrice VehicleTypePrice { get; set; } = null!;
        public double DistanceInMeters { get; set; }
        public double PricePerKm { get; set; }

        public double TotalPrice { get; set; }

        public void CalculateTotalPrice()
        {
            double discount = Discount?.Percent ?? 0;
            if (DistanceInMeters < 0.5)
            {
                TotalPrice = VehicleTypePrice.DropCharge * (1 - discount);
            }
            else
            {
                TotalPrice = (DistanceInMeters * PricePerKm) * (1 - discount);
            }
        }
    }

}
