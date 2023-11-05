using Microsoft.EntityFrameworkCore;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.CodeValue.Grpc.Repository
{
    public class CodeValueRepository : Repository<Entities.CodeValue>, IRepository<Entities.CodeValue>
    {
        private readonly DbContext _db;

        public CodeValueRepository(DbContext db) : base(db)
        {
            _db = db;
        }
    }
}
