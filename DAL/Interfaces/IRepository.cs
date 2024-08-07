using System.Linq.Expressions;

namespace PLMS.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        Task<T> GetByPredicateAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        Task<T> GetByIdAsync<TId>(TId id);
        
        Task CreateAsync(T item);
        
        void Remove(T item);
        
        void RemoveRange(IEnumerable<T> entities);
        
        void Update(T item);

        Task<IQueryable<T>> GetFilteredAsync(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "");
    }
}
