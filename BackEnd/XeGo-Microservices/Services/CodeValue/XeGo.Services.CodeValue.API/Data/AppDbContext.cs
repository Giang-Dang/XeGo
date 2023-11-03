using Microsoft.EntityFrameworkCore;
using XeGo.Services.CodeValue.API.Entities;

namespace XeGo.Services.CodeValue.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Entities.CodeValue> CodeValues { get; set; }
        public DbSet<CodeMetaData> CodeMetaData { get; set; }
    }
}
