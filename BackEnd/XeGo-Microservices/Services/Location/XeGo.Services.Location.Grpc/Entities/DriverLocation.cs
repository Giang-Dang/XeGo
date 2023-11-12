using System.ComponentModel.DataAnnotations;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Location.Grpc.Entities
{
    public class DriverLocation : BaseEntity
    {
        [Required]
        [Key]
        public string UserId { get; set; } = string.Empty!;
        [Required]
        public string Geohash { get; set; } = string.Empty!;
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
    }
}
