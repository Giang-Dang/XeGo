using Microsoft.EntityFrameworkCore;
using XeGo.Services.Ride.API.Entities;
using XeGo.Services.Ride.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.Ride.API.Repository
{
    public class CodeValueRepository : Repository<CodeValue>, ICodeValueRepository
    {
        public CodeValueRepository(DbContext db) : base(db)
        {
        }
    }
}
