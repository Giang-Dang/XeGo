namespace XeGo.Services.Driver.API.Models
{
    public class GetVehicleBanRequestDto
    {
        public int? Id { get; set; }
        public int? VehicleId { get; set; }
        public string? Reason { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedStartDate { get; set; }
        public DateTime? CreatedEndDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedStartDate { get; set; }
        public DateTime? LastModifiedEndDate { get; set; }
        public string? SearchReason { get; set; }
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 0;
    }
}
