using XeGo.Services.Vehicle.API.Data;
using XeGo.Services.Vehicle.API.Entities;
using XeGo.Services.Vehicle.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Vehicle.API.Repository
{
    public class DriverVehicleRepository : Repository<DriverVehicle>, IDriverVehicleRepository
    {
        private readonly AppDbContext _dbContext;

        public DriverVehicleRepository(AppDbContext db) : base(db)
        {
            _dbContext = db;
        }
    }
}
