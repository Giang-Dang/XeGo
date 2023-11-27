using XeGo.Services.Price.API.Data;
using XeGo.Services.Price.API.Entities;
using XeGo.Services.Price.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Price.API.Repository
{
    public class VehicleTypePriceRepository(AppDbContext db) : Repository<VehicleTypePrice>(db), IVehicleTypePriceRepository
    {
        private readonly AppDbContext _dbContext = db;

    }
}
