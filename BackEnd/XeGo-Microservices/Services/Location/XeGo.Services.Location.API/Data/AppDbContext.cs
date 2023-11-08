using Microsoft.EntityFrameworkCore;
using XeGo.Services.Location.API.Entities;

namespace XeGo.Services.Location.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<UserLocation> UserLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserLocation>()
                .HasIndex(p => p.UserId);

            modelBuilder.Entity<UserLocation>()
                .HasIndex(p => p.Geohash);
        }
    }
}
