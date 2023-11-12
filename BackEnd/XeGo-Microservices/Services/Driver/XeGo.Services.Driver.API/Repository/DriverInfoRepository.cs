using XeGo.Services.Driver.API.Data;
using XeGo.Services.Driver.API.Entities;
using XeGo.Services.Driver.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Driver.API.Repository
{
    public class DriverInfoRepository : Repository<DriverInfo>, IDriverInfoRepository
    {
        private readonly AppDbContext _dbContext;

        public DriverInfoRepository(AppDbContext db) : base(db)
        {
            _dbContext = db;
        }
    }
}
