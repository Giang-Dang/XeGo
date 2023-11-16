using XeGo.Services.Vehicle.API.Data;
using XeGo.Services.Vehicle.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Vehicle.API.Repository
{
    public class VehicleRepository : Repository<Vehicle.API.Entities.Vehicle>, IVehicleRepository
    {
        private readonly AppDbContext _dbContext;

        public VehicleRepository(AppDbContext db) : base(db)
        {
            _dbContext = db;
        }
    }
}
