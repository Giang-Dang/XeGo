using Microsoft.EntityFrameworkCore;
using XeGo.Services.Vehicle.API.Entities;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Vehicle.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Vehicle> Vehicles { get; set; }
        public DbSet<VehicleBan> VehicleBans { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VehicleType>()
                .HasData(
                    new VehicleType()
                    {
                        Id = 1,
                        Name = "4-seater Car",
                        CreatedBy = RoleConstants.Admin,
                        LastModifiedBy = RoleConstants.Admin,
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        IsActive = true
                    },
                    new VehicleType()
                    {
                        Id = 2,
                        Name = "7-seater Car",
                        CreatedBy = RoleConstants.Admin,
                        LastModifiedBy = RoleConstants.Admin,
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        IsActive = true
                    }
                );
        }
    }
}
