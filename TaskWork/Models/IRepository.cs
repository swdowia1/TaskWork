using System.Linq.Expressions;

namespace TaskWork.Models
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAllIncludingAsync<T>(params Expression<Func<T, object>>[] includes) where T : class;
        Task<List<T>> GetAllIncludingAsync<T>(params string[] includeProperties ) where T : class;
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdIncludingAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdIncludingAsync(int id, params string[] includeProperties);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
