using Microsoft.EntityFrameworkCore;
using XeGo.Services.Driver.API.Entities;

namespace XeGo.Services.Driver.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<DriverInfo> DriverInfos { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<DriverBan> DriverBans { get; set; }
        public DbSet<VehicleBan> VehicleBans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
                .HasOne(rf => rf.DriverInfo)
                .WithMany()
                .HasForeignKey(rf => rf.DriverId);

            modelBuilder.Entity<DriverBan>()
                .HasOne(b => b.DriverInfo)
                .WithMany()
                .HasForeignKey(b => b.DriverId);

            modelBuilder.Entity<VehicleBan>()
                .HasOne(b => b.Vehicle)
                .WithMany()
                .HasForeignKey(b => b.VehicleId);
        }
    }
}
