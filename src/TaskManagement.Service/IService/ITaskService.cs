namespace TaskManagement.Services.IService
{
    public interface ITaskService
    {
        Task<IEnumerable<Core.Models.Task>> GetAllTasks();

        Task<Core.Models.Task?> GetById(int id);

        void CreateTask(Core.Models.Task task);

        void UpdateTask(Core.Models.Task task);

        void DeleteTask(Core.Models.Task task);
    }
}
