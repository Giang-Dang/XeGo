using Microsoft.EntityFrameworkCore;
using XeGo.Services.CodeValue.Grpc.Entities;
using XeGo.Services.CodeValue.Grpc.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.CodeValue.Grpc.Repository
{
    public class CodeMetaDataRepository : Repository<CodeMetaData>, ICodeMetaData
    {
        private readonly DbContext _dbContext;

        public CodeMetaDataRepository(DbContext db) : base(db)
        {
            _dbContext = db;
        }
    }
}
