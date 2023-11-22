using Microsoft.EntityFrameworkCore;
using XeGo.Services.Ride.API.Entities;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Ride.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Entities.Ride> Rides { get; set; }
        public DbSet<UserConnectionId> UserConnectionIds { get; set; }
        public DbSet<Entities.CodeValue> CodeValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
