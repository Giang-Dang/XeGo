using XeGo.Services.Driver.API.Data;
using XeGo.Services.Driver.API.Entities;
using XeGo.Services.Driver.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Driver.API.Repository
{
    public class VehicleBanRepository : Repository<VehicleBan>, IVehicleBanRepository
    {
        private readonly AppDbContext _dbContext;

        public VehicleBanRepository(AppDbContext db) : base(db)
        {
            _dbContext = db;
        }
    }
}
