using Microsoft.Extensions.Logging;
using PLMS.DAL.Interfaces;
using PMLS.DAL.Entities;
using Task = System.Threading.Tasks.Task;

namespace PLMS.DAL.Implementation
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly LearningDbContext _db;

        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(LearningDbContext db, ILogger<UnitOfWork> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task CommitChangesToDatabaseAsync()
        {
            _logger.LogInformation("Saving changes to the database.");
            await _db.SaveChangesAsync();
            _logger.LogInformation("Changes saved to the database.");
        }
    }
}

