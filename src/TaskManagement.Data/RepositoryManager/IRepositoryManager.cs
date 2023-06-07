using TaskManagement.Data.RepositoryEntity.IRepository;

namespace TaskManagement.Data.RepositoryManager
{
    public interface IRepositoryManager : IDisposable
    {        
        public ITaskRepository TaskRepository { get; }

        Task<int> SaveChanges();
    }
}
