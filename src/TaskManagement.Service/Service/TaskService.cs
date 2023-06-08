using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TaskManagement.Core.DTOs;
using TaskManagement.Core.Enums;
using TaskManagement.Data.RepositoryManager;
using TaskManagement.Services.IService;

namespace TaskManagement.Services.Service
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Core.Models.Task> _tasks;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _tasks = _uow.Set<Core.Models.Task>();
            _mapper = mapper;
        }

        public void CreateTask(Core.Models.Task task)
        {            
            _tasks.Add(task);
        }

        public void DeleteTask(Core.Models.Task task)
        {
            _tasks.Remove(task);
        }

        public async Task<Core.Models.Task?> GetById(int? id)
        {
            return await _tasks.FindAsync(id);
        }

        public async Task<Core.Models.Task?> GetByIdIncludeUser(int? id)
        {
            return await _tasks.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<(IEnumerable<Core.Models.Task> Items, int Count)> GetPagedTaskList(string searchQuery, string sortOrder, int pageNumber, int pageSize)
        {
            var query = _tasks.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                if (Enum.TryParse(searchQuery, true, out Status parsedStatus))
                {
                    query = query.Where(item => item.Status == parsedStatus).AsQueryable();
                }
                else if (Enum.TryParse(searchQuery, true, out Priority parsedPriority))
                {
                    query = query.Where(item => item.Priority == parsedPriority).AsQueryable();
                }
                else if (DateTime.TryParseExact(searchQuery, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate)) //"yyyy-MM-dd" "MM/dd/yyyy"
                {
                    query = query.Where(item => item.DueDate == parsedDate.Date).AsQueryable();
                }
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                // Apply sorting based on the sortOrder parameter
                switch (sortOrder.ToLower())
                {
                    case "priority_desc":
                        query = query.OrderByDescending(item => item.Priority).AsQueryable();
                        break;
                    case "priority_asc":
                        query = query.OrderBy(item => item.Priority).AsQueryable();
                        break;
                    case "status_desc":
                        query = query.OrderByDescending(item => item.Status).AsQueryable();
                        break;
                    case "status_asc":
                        query = query.OrderBy(item => item.Status).AsQueryable();
                        break;
                    case "duedate_desc":
                        query = query.OrderByDescending(item => item.DueDate).AsQueryable();
                        break;
                    case "duedate_asc":
                        query = query.OrderBy(item => item.DueDate).AsQueryable();
                        break;
                    default:
                        query = query.OrderBy(item => item.Title).AsQueryable();
                        break;
                }
            }

            query = query.Include(x => x.User).AsQueryable();

            // Get the count of items
            int count = await query.CountAsync();
            // Get the list of items
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            //List<PagedTaskListDto> items = _mapper.ProjectTo<PagedTaskListDto>(list.AsQueryable()).ToList();

            return (items, count);
        }

        public bool TaskExists(int id)
        {
            return (_tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public void UpdateTask(Core.Models.Task task)
        {
            _tasks.Update(task);
        }
    }
}
