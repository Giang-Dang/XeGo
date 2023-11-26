using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Price.API.Entities
{
    public class Discount : BaseEntity
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public double Percent { get; set; }
        public int Quantity { get; set; }
        public DateTime FromDay { get; set; }
        public DateTime ToDay { get; set; }
    }
}
