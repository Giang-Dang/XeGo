using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Auth.API.Entities
{
    public class RoleFunction : BaseEntity
    {
        [ForeignKey("Role")]
        public string RoleId { get; set; } = String.Empty!;
        public IdentityRole Role { get; set; }

        [ForeignKey("Function")]
        public int FunctionId { get; set; }
        public Function Function { get; set; }
    }
}
