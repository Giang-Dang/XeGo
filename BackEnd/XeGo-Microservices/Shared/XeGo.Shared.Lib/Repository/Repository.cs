using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace XeGo.Shared.Lib.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _db;
        private readonly DbSet<T?> _dbSet;
        public Repository(DbContext db)
        {
            _db = db;
            this._dbSet = _db.Set<T>();
        }

        public async Task<T?> CreateAsync(T? entity)
        {
            _dbSet.Add(entity);
            await SaveAsync();
            return entity;
        }

        public async Task<T?> UpdateAsync(T? entity)
        {
            _dbSet.Update(entity);
            await SaveAsync();
            return entity;
        }


        public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T?> query = _dbSet;
            if (!tracked)
            {
                query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter!);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T?>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
            int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T?> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter!);
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T?, bool>>? predicate = null)
        {
            if (predicate != null)
            {
                return await _dbSet.AnyAsync(predicate);
            }
            return await _dbSet.AnyAsync();
        }

        public async Task RemoveAsync(T? entity)
        {
            _dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
