using XeGo.Services.Driver.API.Data;
using XeGo.Services.Driver.API.Entities;
using XeGo.Services.Driver.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Driver.API.Repository
{
    public class BanRepository : Repository<DriverBan>, IBanRepository
    {
        private readonly AppDbContext _dbContext;

        public BanRepository(AppDbContext db) : base(db)
        {
            _dbContext = db;
        }
    }
}
