using Microsoft.EntityFrameworkCore;
using XeGo.Services.Vehicle.API.Entities;

namespace XeGo.Services.Vehicle.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Vehicle> Vehicles { get; set; }
        public DbSet<VehicleBan> VehicleBans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
