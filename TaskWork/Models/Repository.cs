using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TaskWork.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        private readonly DbSet<T> _set;

        public Repository(AppDbContext db)
        {
            _db = db;
            _set = db.Set<T>();
        }

        public Task<List<T>> GetAllAsync() => _set.ToListAsync();
        public Task<T?> GetByIdAsync(int id) => _set.FindAsync(id).AsTask();

        public async Task AddAsync(T entity) => await _set.AddAsync(entity);
        public Task UpdateAsync(T entity) { _set.Update(entity); return Task.CompletedTask; }
        public async Task DeleteAsync(int id)
        {
            var e = await GetByIdAsync(id);
            if (e != null) _set.Remove(e);
        }
        public Task SaveChangesAsync() => _db.SaveChangesAsync();

        public async Task<List<T1>> GetAllIncludingAsync<T1>(params Expression<Func<T1, object>>[] includes) where T1 : class
        {
            IQueryable<T1> query = _db.Set<T1>();

            // dodajemy Includes, jeśli są podane
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }
    }
}
