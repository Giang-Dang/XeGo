using Microsoft.EntityFrameworkCore;
using XeGo.Services.Location.API.Entities;

namespace XeGo.Services.Location.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<DriverLocation> DriverLocations { get; set; }
        public DbSet<RiderLocation> RiderLocations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DriverLocation>()
                .HasIndex(p => p.UserId);

            modelBuilder.Entity<DriverLocation>()
                .HasIndex(p => p.Geohash);

            modelBuilder.Entity<RiderLocation>()
                .HasIndex(p => p.UserId);

            modelBuilder.Entity<RiderLocation>()
                .HasIndex(p => p.Geohash);
        }
    }
}
