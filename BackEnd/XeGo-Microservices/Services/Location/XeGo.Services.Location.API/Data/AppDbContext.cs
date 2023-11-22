using Microsoft.EntityFrameworkCore;
using XeGo.Services.Location.API.Entities;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Location.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<DriverLocation> DriverLocations { get; set; }
        public DbSet<RiderLocation> RiderLocations { get; set; }
        public DbSet<Entities.CodeValue> CodeValues { get; set; }

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

            modelBuilder.Entity<Entities.CodeValue>()
                .HasData(
                    new Entities.CodeValue()
                    {
                        Id = 11,
                        Name = GeohashConstants.GeohashName,
                        SortOrder = 1,
                        EffectiveStartDate = DateTime.UtcNow,
                        EffectiveEndDate = DateTime.MaxValue,
                        Value1 = GeohashConstants.GeoHashSquareSideInMeters,
                        Value1Type = CodeValueTypeConstants.String,
                        Value2 = "500",
                        Value2Type = CodeValueTypeConstants.Double,
                        CreatedBy = RoleConstants.Admin,
                        LastModifiedBy = RoleConstants.Admin,
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                    },
                    new Entities.CodeValue()
                    {
                        Id = 12,
                        Name = GeohashConstants.GeohashName,
                        SortOrder = 1,
                        EffectiveStartDate = DateTime.UtcNow,
                        EffectiveEndDate = DateTime.MaxValue,
                        Value1 = GeohashConstants.MaxRadiusInMetersName,
                        Value1Type = CodeValueTypeConstants.String,
                        Value2 = "1000",
                        Value2Type = CodeValueTypeConstants.Double,
                        CreatedBy = RoleConstants.Admin,
                        LastModifiedBy = RoleConstants.Admin,
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                    }
                );
        }
    }
}
