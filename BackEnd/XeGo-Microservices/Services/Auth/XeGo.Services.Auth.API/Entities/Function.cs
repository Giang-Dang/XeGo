using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Auth.API.Entities
{
    public class Function : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty!;
    }
}
