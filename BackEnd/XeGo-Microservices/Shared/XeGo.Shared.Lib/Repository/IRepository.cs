using System.Linq.Expressions;

namespace XeGo.Shared.Lib.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T?>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
            int pageSize = 0, int pageNumber = 1);
        Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null);
        Task<T?> CreateAsync(T? entity);
        Task<T?> UpdateAsync(T? entity);
        Task<bool> AnyAsync(Expression<Func<T?, bool>>? predicate = null);
        Task RemoveAsync(T? entity);
        Task SaveAsync();
    }
}
