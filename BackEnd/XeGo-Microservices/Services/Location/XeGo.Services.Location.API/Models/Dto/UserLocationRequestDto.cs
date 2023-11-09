namespace XeGo.Services.Location.API.Models.Dto
{
    public class UserLocationRequestDto
    {
        public string UserId { get; set; } = string.Empty!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsDriver { get; set; } = true;
    }
}
