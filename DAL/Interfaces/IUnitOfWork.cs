namespace PLMS.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        Task CommitChangesToDatabaseAsync();
    }
}
