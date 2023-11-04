using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XeGo.Services.Auth.API.Entities;

namespace XeGo.Services.Auth.API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<RoleFunction> RoleFunction { get; set; }
        public DbSet<Function> Functions { get; set; }

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
        }
    }
}
