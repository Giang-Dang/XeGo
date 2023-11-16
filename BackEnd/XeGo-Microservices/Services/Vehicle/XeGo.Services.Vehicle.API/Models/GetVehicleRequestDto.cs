namespace XeGo.Services.Vehicle.API.Models
{
    public class GetVehicleRequestDto
    {
        public int? Id { get; set; }
        public string? PlateNumber { get; set; }
        public string? Type { get; set; }
        public string? DriverId { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedStartDate { get; set; }
        public DateTime? CreatedEndDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedStartDate { get; set; }
        public DateTime? LastModifiedEndDate { get; set; }
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public string? SearchString { get; set; }
    }
}
