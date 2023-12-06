using Microsoft.EntityFrameworkCore;
using XeGo.Services.Vehicle.Grpc.Entities;

namespace XeGo.Services.Vehicle.Grpc.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Vehicle> Vehicles { get; set; }
        public DbSet<VehicleBan> VehicleBans { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DriverVehicle> DriverVehicles { get; set; }

    }
}
