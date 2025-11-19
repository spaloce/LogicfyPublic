using System.Linq.Expressions;

namespace Logicfy.Data.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        IQueryable<T> Query(); // gelişmiş sorgular için
    }
}
