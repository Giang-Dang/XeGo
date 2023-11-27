using XeGo.Services.Price.API.Data;
using XeGo.Services.Price.API.Entities;
using XeGo.Services.Price.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Price.API.Repository
{
    public class DiscountRepository(AppDbContext db) : Repository<Discount>(db), IDiscountRepository
    {
        private readonly AppDbContext _dbContext = db;
    }
}
