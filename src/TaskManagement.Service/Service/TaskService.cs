using Microsoft.EntityFrameworkCore;
using TaskManagement.Data.RepositoryManager;
using TaskManagement.Services.IService;

namespace TaskManagement.Services.Service
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _uow;

        private readonly DbSet<Core.Models.Task> _tasks;

        public TaskService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));

            _tasks = _uow.Set<Core.Models.Task>();
        }

        public void CreateTask(Core.Models.Task task)
        {
            _tasks.Add(task);
        }

        public void DeleteTask(Core.Models.Task task)
        {
            _tasks.Remove(task);
        }

        public async Task<IEnumerable<Core.Models.Task>> GetAllTasks()
        {
            return await _tasks.ToListAsync();
        }

        public async Task<Core.Models.Task?> GetById(int id)
        {
            return await _tasks.FindAsync(id);
        }

        public void UpdateTask(Core.Models.Task task)
        {
            _tasks.Update(task);
        }
    }
}
