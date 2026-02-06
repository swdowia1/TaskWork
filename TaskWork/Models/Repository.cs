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
        public async Task<T?> GetByIdIncludingAsync(int id, params string[] includeProperties)
        {
            IQueryable<T> query = _set;

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            // zakładamy, że klasa ma właściwość "Id"
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<T?> GetByIdIncludingAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set;

            foreach (var include in includes)
                query = query.Include(include);

            // Zakładamy, że klasa T ma właściwość "Id"
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

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
        public async Task<List<T>> GetAllIncludingAsync<T>(
    params string[] includePaths
) where T : class
        {
            IQueryable<T> query = _db.Set<T>();

            foreach (var path in includePaths)
            {
                query = query.Include(path);
            }

            return await query.ToListAsync();
        }
    }
}
