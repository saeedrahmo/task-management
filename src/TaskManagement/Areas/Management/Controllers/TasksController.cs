﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Areas.Management.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [Area("Management")]
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TasksController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Management/Tasks
        public async Task<IActionResult> Index(string searchQuery, string sortOrder)
        {
            if (_context.Task == null)
                return Problem("Entity set 'AppDbContext.Task'  is null.");

            var query = _context.Task.AsQueryable();
            ViewData["SortOrder"] = sortOrder;

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


            var list = await query.Include(x => x.User).Select(x => new Models.Task
            {
                Id = x.Id,
                Title = x.Title,
                Assignee = x.User != null ? x.User.UserName : string.Empty,
                DueDate = x.DueDate,
                Priority = x.Priority,
                Status = x.Status,
                UserId = x.UserId,
                Description = x.Description,
            }).ToListAsync();

            return View(list);
        }

        // GET: Management/Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            //var task = await _context.Task
            //    .FirstOrDefaultAsync(m => m.Id == id);

            var task = await _context.Task.Include(x => x.User).Select(x => new TaskManagement.Models.Task
            {
                Id = x.Id,
                Title = x.Title,
                Assignee = x.User != null ? x.User.UserName : string.Empty,
                DueDate = x.DueDate,
                Priority = x.Priority,
                Status = x.Status,
                //UserId = x.UserId,
                Description = x.Description,
            }).FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Management/Tasks/Create
        public async Task<IActionResult> Create()
        {

            var users = await _userManager.Users.Select(x => new ApplicationUser { Id = x.Id, UserName = x.UserName }).ToListAsync();
            var task = new TaskManagement.Models.Task() { Users = users, };

            return View(task);
        }

        // POST: Management/Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,DueDate,Priority,Status,Assignee,UserId")] TaskManagement.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Management/Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }
            
            var task = await _context.Task
               .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null)
            {
                return NotFound();
            }
            var users = await _userManager.Users.Select(x => new ApplicationUser { Id = x.Id, UserName = x.UserName }).ToListAsync();
            task.Users = users;

            return View(task);
        }

        // POST: Management/Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DueDate,Priority,Status,Assignee,UserId")] TaskManagement.Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
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
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            //var task = await _context.Task
            //    .FirstOrDefaultAsync(m => m.Id == id);

            var task = await _context.Task.Include(x => x.User).Select(x => new TaskManagement.Models.Task
            {
                Id = x.Id,
                Title = x.Title,
                Assignee = x.User != null ? x.User.UserName : string.Empty,
                DueDate = x.DueDate,
                Priority = x.Priority,
                Status = x.Status,
                //UserId = x.UserId,
                Description = x.Description,
            }).FirstOrDefaultAsync(x => x.Id == id);

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
            if (_context.Task == null)
            {
                return Problem("Entity set 'AppDbContext.Task'  is null.");
            }
            var task = await _context.Task.FindAsync(id);
            if (task != null)
            {
                _context.Task.Remove(task);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return (_context.Task?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}