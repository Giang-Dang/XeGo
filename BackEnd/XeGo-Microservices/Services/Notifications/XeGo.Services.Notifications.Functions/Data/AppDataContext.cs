using Microsoft.EntityFrameworkCore;
using XeGo.Services.Notifications.Functions.Entities;

namespace XeGo.Services.Notifications.Functions.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<UserConnectionInfo> UserConnectionInfos { get; set; }
        public DbSet<ScheduledTask> ScheduledTasks { get; set; }
    }
}
