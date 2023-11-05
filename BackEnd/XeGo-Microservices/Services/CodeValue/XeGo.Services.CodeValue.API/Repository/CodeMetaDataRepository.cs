using Microsoft.EntityFrameworkCore;
using XeGo.Services.CodeValue.API.Entities;
using XeGo.Services.CodeValue.API.Repository.IRepository;
using XeGo.Shared.Lib.Repository;

namespace XeGo.Services.CodeValue.API.Repository
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
