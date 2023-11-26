using Microsoft.EntityFrameworkCore;
using XeGo.Services.Price.API.Entities;
using XeGo.Services.Price.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Price.API.Repository
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        public DiscountRepository(DbContext db) : base(db)
        {
        }
    }
}
