namespace XeGo.Services.Rider.API.Models
{
    public class GetRiderBanRequestDto
    {
        public string? RiderId { get; set; }
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
        public int PageSize { get; set; } = 0;
        public int PageNumber { get; set; } = 0;
    }

}
