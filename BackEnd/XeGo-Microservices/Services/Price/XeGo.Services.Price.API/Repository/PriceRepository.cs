using Microsoft.EntityFrameworkCore;
using XeGo.Services.Price.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Price.API.Repository
{
    public class PriceRepository : Repository<Entities.Price>, IPriceRepository
    {
        public PriceRepository(DbContext db) : base(db)
        {
        }
    }
}
