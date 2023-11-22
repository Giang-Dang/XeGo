using XeGo.Services.Vehicle.API.Data;
using XeGo.Services.Vehicle.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Vehicle.API.Repository
{
    public class VehicleTypeRepository : Repository<Entities.VehicleType>, IVehicleTypeRepository
    {
        private readonly AppDbContext _dbContext;

        public VehicleTypeRepository(AppDbContext db) : base(db)
        {
            _dbContext = db;
        }
    }
}
