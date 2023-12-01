using Microsoft.EntityFrameworkCore;
using XeGo.Services.Price.Grpc.Entities;

namespace XeGo.Services.Price.Grpc.Data
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
        }
    }
}
