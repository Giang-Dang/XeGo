using Microsoft.EntityFrameworkCore;
using XeGo.Services.Report.Functions.Entities;

namespace XeGo.Services.Report.Functions.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ReportInfo> ReportInfos { get; set; }
    }
}
