namespace XeGo.Services.Report.Functions.Models
{
    public class ParamsReportRequest
    {
        public string ReportType { get; set; } = string.Empty!;
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Year { get; set; }
        public string CreatedBy { get; set; } = string.Empty!;

    }
}
