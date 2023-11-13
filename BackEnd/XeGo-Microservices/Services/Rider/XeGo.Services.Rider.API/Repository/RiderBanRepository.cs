using XeGo.Services.Rider.API.Data;
using XeGo.Services.Rider.API.Entities;
using XeGo.Services.Rider.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Rider.API.Repository
{
    public class RiderBanRepository : Repository<RiderBan>, IRiderBanRepository
    {
        private readonly AppDbContext _dbContext;

        public RiderBanRepository(AppDbContext db) : base(db)
        {
            _dbContext = db;
        }
    }
}
