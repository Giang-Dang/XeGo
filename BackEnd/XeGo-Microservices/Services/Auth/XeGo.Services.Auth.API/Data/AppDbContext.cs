using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XeGo.Services.Auth.API.Entities;

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
                    EffectiveStartDate = DateTime.Parse("2023-11-04T00:00:00.0000000"),
                    EffectiveEndDate = DateTime.Parse("9999-12-31T00:00:00.0000000"),
                    Value1 = "ACCESS_TOKEN_DAYS_TO_EXPIRE",
                    Value1Type = "STRING",
                    Value2 = "7",
                    Value2Type = "INT",
                    IsActive = true,
                    CreatedBy = "ADMIN",
                    CreatedDate = DateTime.Parse("2023-11-04T00:00:00.0000000"),
                    LastModifiedBy = "ADMIN",
                    LastModifiedDate = DateTime.Parse("2023-11-04T00:00:00.0000000")
                }
            );
        }
    }
}
