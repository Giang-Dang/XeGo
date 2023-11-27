using XeGo.Services.Ride.API.Data;
using XeGo.Services.Ride.API.Entities;
using XeGo.Services.Ride.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Ride.API.Repository
{
    public class CodeValueRepository(AppDbContext db) : Repository<CodeValue>(db), ICodeValueRepository
    {
        private readonly AppDbContext _dbContext = db;
    }
}
