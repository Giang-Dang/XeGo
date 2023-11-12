namespace XeGo.Services.Location.API.Models.Dto
{
    public class RiderLocationRequestDto
    {
        public string UserId { get; set; } = string.Empty!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? CreatedBy { get; set; }
        public string ModifiedBy { get; set; } = "N/A";
    }
}
