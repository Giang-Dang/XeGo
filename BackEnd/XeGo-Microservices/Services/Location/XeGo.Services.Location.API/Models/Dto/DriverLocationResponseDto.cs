namespace XeGo.Services.Location.API.Models.Dto
{
    public class DriverLocationResponseDto
    {
        public string UserId { get; set; } = string.Empty!;
        public string Geohash { get; set; } = string.Empty!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
