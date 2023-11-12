using XeGo.Services.Driver.API.Data;
using XeGo.Services.Driver.API.Entities;
using XeGo.Services.Driver.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Driver.API.Repository
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        private readonly AppDbContext _dbContext;

        public VehicleRepository(AppDbContext db) : base(db)
        {
            _dbContext = db;
        }
    }
}
