using Microsoft.Extensions.Logging;
using PLMS.DAL.Interfaces;
using PLMS.DAL.Entities;
using Task = System.Threading.Tasks.Task;

namespace PLMS.DAL.Implementation
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly LearningDbContext _db;

        private readonly Dictionary<Type, object> _repositories = new();

        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(LearningDbContext db, ILogger<UnitOfWork> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IRepository<T>)_repositories[typeof(T)];
            }

            var repository = new Repository<T>(_db);
            _repositories.Add(typeof(T), repository);
            return repository;
        }

        public async Task CommitChangesToDatabaseAsync()
        {
            _logger.LogInformation("Saving changes to the database.");
            await _db.SaveChangesAsync();
            _logger.LogInformation("Changes saved to the database.");
        }
    }
}

