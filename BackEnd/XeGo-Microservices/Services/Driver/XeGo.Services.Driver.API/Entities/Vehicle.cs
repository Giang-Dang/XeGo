using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Driver.API.Entities
{
    public class Vehicle : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string PlateNumber { get; set; } = string.Empty!;
        [Required] public string Type { get; set; } = string.Empty!;
        [Required] public string DriverId { get; set; } = string.Empty!;
        [Required] public DriverInfo DriverInfo { get; set; } = null!;
    }
}
