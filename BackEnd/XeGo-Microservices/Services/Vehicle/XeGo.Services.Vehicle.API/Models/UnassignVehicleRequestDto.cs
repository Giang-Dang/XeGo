using System.ComponentModel.DataAnnotations;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Vehicle.API.Models
{
    public class UnassignVehicleRequestDto
    {
        [Required] public int Id { get; set; }
        [Required] public string ModifiedBy { get; set; } = RoleConstants.Admin;
    }
}
