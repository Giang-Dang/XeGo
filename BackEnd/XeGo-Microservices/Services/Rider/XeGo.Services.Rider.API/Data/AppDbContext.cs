using Microsoft.EntityFrameworkCore;
using XeGo.Services.Rider.API.Entities;

namespace XeGo.Services.Rider.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<RiderInfo> RiderInfos { get; set; }
        public DbSet<RiderBan> RiderBans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RiderBan>()
                .HasOne(b => b.RiderInfo)
                .WithMany()
                .HasForeignKey(b => b.RiderId);
        }
    }
}
