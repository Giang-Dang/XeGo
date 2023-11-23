using Microsoft.EntityFrameworkCore;
using XeGo.Services.Ride.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Ride.API.Repository
{
    public class RideRepository : Repository<Entities.Ride>, IRideRepository
    {
        public RideRepository(DbContext db) : base(db)
        {
        }
    }
}
