using Microsoft.EntityFrameworkCore;
using XeGo.Services.File.API.Entities;

namespace XeGo.Services.File.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<UserFiles> UserFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
