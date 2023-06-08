using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Controllers;
using TaskManagement.Core.DTOs;
using TaskManagement.Data.RepositoryManager;
using TaskManagement.Services.IService;

namespace TaskManagement.Areas.Management.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [Area("Management")]
    public class TasksController : BaseController<TasksController>
    {        
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;

        public TasksController(ILogger<TasksController> logger, IUnitOfWork uow, IMapper mapper, ITaskService taskService, IUserService userService) : base(logger, uow, mapper)
        {            
            _taskService = taskService;
            _userService = userService;            
        }
        
        // GET: Management/Tasks
        public async Task<IActionResult> Index(string searchQuery, string sortOrder, int pageNumber = 1, int pageSize = 3)
        {
            // TODO: limit list by userId if the user is not admin

            //return Problem("Entity set 'AppDbContext.Task'  is null.");

            ViewData["SortOrder"] = sortOrder;
            ViewData["PageNumber"] = pageNumber;
            ViewData["PageSize"] = pageSize;

            var result = await _taskService.GetPagedTaskList(searchQuery, sortOrder, pageNumber, pageSize);
            var totalCount = result.Count;
            var items = result.Items;

            List<PagedTaskListDto> list = _mapper.ProjectTo<PagedTaskListDto>(items.AsQueryable()).ToList();

            ViewData["TotalCount"] = totalCount;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(list);
        }

        // GET: Management/Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _taskService.GetByIdIncludeUser(id);
            var task = _mapper.Map<ReadTaskDto>(obj);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Management/Tasks/Create
        public async Task<IActionResult> Create()
        {
            // TODO: limit action's access to the user with admin role 

            var users = await _userService.GetUserList();
            List<UserListDto> usersDto = _mapper.ProjectTo<UserListDto>(users.AsQueryable()).ToList();
            var task = new CreateTaskDto() { Users = usersDto };

            return View(task);
        }

        // POST: Management/Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,DueDate,Priority,Status,Assignee")] CreateTaskDto task)
        {
            if (ModelState.IsValid)
            {
                Core.Models.Task obj = _mapper.Map<Core.Models.Task>(task);
                _taskService.CreateTask(obj);
                await _uow.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Management/Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _taskService.GetById(id);
            var task = _mapper.Map<UpdateTaskDto>(obj);

            if (task == null)
            {
                return NotFound();
            }

            var users = await _userService.GetUserList();
            List<UserListDto> usersDto = _mapper.ProjectTo<UserListDto>(users.AsQueryable()).ToList();
            task.Users = usersDto;

            return View(task);
        }

        // POST: Management/Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DueDate,Priority,Status,Assignee,UserId")] UpdateTaskDto task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var obj = _mapper.Map<Core.Models.Task>(task);
                    _taskService.UpdateTask(obj);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_taskService.TaskExists(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Management/Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _taskService.GetByIdIncludeUser(id);
            var task = _mapper.Map<DeleteTaskDto>(obj);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Management/Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            //return Problem("Entity set 'AppDbContext.Task'  is null.");

            var task = await _taskService.GetById(id);
            if (task != null)
            {
                _taskService.DeleteTask(task);
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
