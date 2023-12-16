using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Report.Functions.Entities
{
    public class ReportInfo : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string ReportType { get; set; } = string.Empty!;
        [Required] public string ReportName { get; set; } = string.Empty!;
        [Required] public DateTime SubmissionDateTime { get; set; }
        public DateTime? CompletionDateTime { get; set; }
        [Required] public string Status { get; set; } = string.Empty!;
    }
}
