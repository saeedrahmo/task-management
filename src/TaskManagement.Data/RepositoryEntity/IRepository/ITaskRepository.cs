using TaskManagement.Data.RepositoryBase;

namespace TaskManagement.Data.RepositoryEntity.IRepository
{
    public interface ITaskRepository : IGenericRepository<Core.Models.Task>
    {
        Task<IEnumerable<Core.Models.Task>> GetAllTasks();

        Task<Core.Models.Task?> GetById(int id);

        void CreateTask(Core.Models.Task task);

        void UpdateTask(Core.Models.Task task);

        void DeleteTask(Core.Models.Task task);
    }
}
