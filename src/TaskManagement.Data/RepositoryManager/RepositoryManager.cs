using TaskManagement.Data.EF;
using TaskManagement.Data.RepositoryEntity.IRepository;

namespace TaskManagement.Data.RepositoryManager
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationDbContext _context;

        public ITaskRepository TaskRepository { get; }

        public RepositoryManager(ApplicationDbContext context, ITaskRepository taskRepository)
        {
            _context = context;
            TaskRepository = taskRepository;
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
