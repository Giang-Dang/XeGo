using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.CodeValue.Grpc.Models.Dto
{
    public class CodeMetaDataDto : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty!;
        public string? Description { get; set; }
        public string? Value1Name { get; set; }
        public string? Value1Type { get; set; }
        public string? Value2Name { get; set; }
        public string? Value2Type { get; set; }
        public string? Value3Name { get; set; }
        public string? Value3Type { get; set; }
        public string? Value4Name { get; set; }
        public string? Value4Type { get; set; }
        public string? Value5Name { get; set; }
        public string? Value5Type { get; set; }
        public string? Value6Name { get; set; }
        public string? Value6Type { get; set; }
        public string? Value7Name { get; set; }
        public string? Value7Type { get; set; }
        public string? Value8Name { get; set; }
        public string? Value8Type { get; set; }
        public string? Value9Name { get; set; }
        public string? Value9Type { get; set; }
        public string? Value10Name { get; set; }
        public string? Value10Type { get; set; }
    }
}
