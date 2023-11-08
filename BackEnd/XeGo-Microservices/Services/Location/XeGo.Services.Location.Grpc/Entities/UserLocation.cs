using System.ComponentModel.DataAnnotations;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Location.API.Entities
{
    public class UserLocation : BaseEntity
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Geohash { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public bool IsDriver { get; set; }
    }
}
