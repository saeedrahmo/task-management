using TaskManagement.Core.DTOs;

namespace TaskManagement.Services.IService
{
    public interface ITaskService
    {        
        Task<(IEnumerable<Core.Models.Task> Items, int Count)> GetPagedTaskList(string searchQuery, string sortOrder, int pageNumber, int pageSize);
        
        Task<Core.Models.Task?> GetById(int? id);

        Task<Core.Models.Task?> GetByIdIncludeUser(int? id);

        void CreateTask(Core.Models.Task task);

        void UpdateTask(Core.Models.Task task);

        void DeleteTask(Core.Models.Task task);

        bool TaskExists(int id);
    }
}
