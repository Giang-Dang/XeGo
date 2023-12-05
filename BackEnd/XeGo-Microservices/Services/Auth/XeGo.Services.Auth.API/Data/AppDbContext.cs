using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XeGo.Services.Auth.API.Entities;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Auth.API.Data
{
    public class AppDbContext :
        IdentityDbContext<
            ApplicationUser,
            IdentityRole,
            string,
            IdentityUserClaim<string>,
            IdentityUserRole<string>,
            IdentityUserLogin<string>,
            IdentityRoleClaim<string>,
            ApplicationUserToken>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationUserToken> ApplicationTokens { get; set; }
        public DbSet<RoleFunction> RoleFunction { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<CodeValue> CodeValues { get; set; }
        public DbSet<Rider> Riders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoleFunction>()
                .HasOne(rf => rf.Role)
                .WithMany()
                .HasForeignKey(rf => rf.RoleId);

            modelBuilder.Entity<RoleFunction>()
                .HasOne(rf => rf.Function)
                .WithMany()
                .HasForeignKey(rf => rf.FunctionId);

            modelBuilder.Entity<CodeValue>().HasData(
                new CodeValue
                {
                    Id = 9,
                    Name = "TOKEN_PROPERTY",
                    SortOrder = 1,
                    EffectiveStartDate = DateTime.UtcNow,
                    EffectiveEndDate = DateTime.MaxValue,
                    Value1 = "ACCESS_TOKEN_DAYS_TO_EXPIRE",
                    Value1Type = CodeValueTypeConstants.String,
                    Value2 = "7",
                    Value2Type = CodeValueTypeConstants.Int,
                    IsActive = true,
                    CreatedBy = RoleConstants.Admin,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedBy = RoleConstants.Admin,
                    LastModifiedDate = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
                new IdentityRole
                {
                    Name = "Driver",
                    NormalizedName = "DRIVER"
                },
                new IdentityRole
                {
                    Name = "Rider",
                    NormalizedName = "RIDER"
                },
                new IdentityRole
                {
                    Name = "Staff",
                    NormalizedName = "STAFF"
                });
        }
    }
}
