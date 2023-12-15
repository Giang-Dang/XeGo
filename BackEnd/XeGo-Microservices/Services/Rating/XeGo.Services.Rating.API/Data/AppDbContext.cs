using Microsoft.EntityFrameworkCore;
using XeGo.Services.Rating.API.Entities;

namespace XeGo.Services.Rating.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<UserAverageRating> UserAverageRatings { get; set; }
    }
}
