using XeGo.Services.Vehicle.API.Data;
using XeGo.Services.Vehicle.API.Entities;
using XeGo.Services.Vehicle.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Vehicle.API.Repository
{
    public class DriverRepository : Repository<Driver>, IDriverRepository
    {
        private readonly AppDbContext _dbContext;

        public DriverRepository(AppDbContext db) : base(db)
        {
            _dbContext = db;
        }
    }
}
