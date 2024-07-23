using Microsoft.EntityFrameworkCore;
using PLMS.DAL.Extensions;
using PLMS.DAL.Interfaces;
using PMLS.DAL.Entities;
using System.Linq.Expressions;

namespace PLMS.DAL.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LearningDbContext _db;

        public Repository(LearningDbContext db)
        {
            _db = db;
        }

        public IQueryable<T> GetAll() => _db.Set<T>().AsQueryable();

        public Task<T> GetByPredicateAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return _db.Set<T>().IncludeMultiple(includes).FirstOrDefaultAsync(predicate);
        }

        public async Task<T> GetByIdAsync<TId>(TId id) => await _db.Set<T>().FindAsync(id);

        public void Create(T item) => _db.Set<T>().Add(item);

        public void Remove(T item) => _db.Set<T>().Remove(item);

        public void RemoveRange(IEnumerable<T> entities) => _db.Set<T>().RemoveRange(entities);

        public void Update(T item) => _db.Entry(item).State = EntityState.Modified;
    }
}
