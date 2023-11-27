using XeGo.Services.Ride.API.Data;
using XeGo.Services.Ride.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Ride.API.Repository
{
    public class RideRepository(AppDbContext db) : Repository<Entities.Ride>(db), IRideRepository
    {
        private readonly AppDbContext _dbContext = db;
    }
}
