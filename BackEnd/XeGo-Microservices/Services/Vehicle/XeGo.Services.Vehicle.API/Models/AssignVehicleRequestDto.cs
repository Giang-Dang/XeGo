using System.ComponentModel.DataAnnotations;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Vehicle.API.Models
{
    public class AssignVehicleRequestDto
    {
        [Required] public string DriverId { get; set; } = string.Empty!;
        [Required] public int VehicleId { get; set; }
        [Required] public string ModifiedBy { get; set; } = RoleConstants.Admin;
    }
}
