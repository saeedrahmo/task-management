using Microsoft.EntityFrameworkCore;
using TaskManagement.Data.EF;
using TaskManagement.Data.RepositoryBase;
using TaskManagement.Data.RepositoryEntity.IRepository;

namespace TaskManagement.Data.RepositoryEntity.Repository
{
    public class TaskRepository : GenericRepository<Core.Models.Task>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext context) : base(context) { }

        public void CreateTask(Core.Models.Task task)
        {
            _context.Task.Add(task);
        }

        public void DeleteTask(Core.Models.Task task)
        {
            _context.Task.Remove(task);
        }

        public async Task<IEnumerable<Core.Models.Task>> GetAllTasks()
        {
            return await _context.Task.ToListAsync();
        }

        public async Task<Core.Models.Task?> GetById(int id)
        {
            return await _context.Task.FindAsync(id);
        }

        public void UpdateTask(Core.Models.Task task)
        {
            _context.Task.Update(task);
        }
    }
}
