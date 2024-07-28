namespace PLMS.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;
        Task CommitChangesToDatabaseAsync();
    }
}
