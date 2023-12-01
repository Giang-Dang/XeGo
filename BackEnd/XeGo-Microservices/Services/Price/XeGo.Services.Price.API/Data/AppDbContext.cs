using Microsoft.EntityFrameworkCore;
using XeGo.Services.Price.API.Entities;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Price.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Entities.Price> Prices { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<VehicleTypePrice> VehicleTypePrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VehicleTypePrice>()
                .HasData(
                    new VehicleTypePrice()
                    {
                        VehicleTypeId = 1,
                        PricePerKm = 1,
                        DropCharge = 1,
                        CreatedBy = RoleConstants.System,
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedBy = RoleConstants.System,
                        LastModifiedDate = DateTime.UtcNow,
                    },
                    new VehicleTypePrice()
                    {
                        VehicleTypeId = 2,
                        PricePerKm = 1.5,
                        DropCharge = 1.5,
                        CreatedBy = RoleConstants.System,
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedBy = RoleConstants.System,
                        LastModifiedDate = DateTime.UtcNow,
                    });
        }
    }
}
