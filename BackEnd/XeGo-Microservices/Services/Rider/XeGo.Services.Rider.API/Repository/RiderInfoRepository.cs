using XeGo.Services.Rider.API.Data;
using XeGo.Services.Rider.API.Entities;
using XeGo.Services.Rider.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Rider.API.Repository
{
    public class RiderInfoRepository : Repository<RiderInfo>, IRiderInfoRepository
    {
        private readonly AppDbContext _dbContext;

        public RiderInfoRepository(AppDbContext db) : base(db)
        {
            _dbContext = db;
        }
    }
}
