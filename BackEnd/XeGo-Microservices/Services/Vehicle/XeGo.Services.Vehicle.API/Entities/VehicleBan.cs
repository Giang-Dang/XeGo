﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Vehicle.API.Entities
{
    public class VehicleBan : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public int VehicleId { get; set; }
        [ForeignKey($"{nameof(VehicleId)}")] public virtual Vehicle Vehicle { get; set; } = null!;
        [Required] public string Reason { get; set; } = string.Empty!;
        [Required] public DateTime StartTime { get; set; }
        [Required] public DateTime EndTime { get; set; }
        [Required] public bool IsActive { get; set; } = true;
    }
}
