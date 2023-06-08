using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.DTOs;
using TaskManagement.Core.Models;
using TaskManagement.Services.IService;

namespace TaskManagement.Services.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUserList()
        {
            return await _userManager.Users.Select(x => new ApplicationUser { Id = x.Id, UserName = x.UserName }).ToListAsync();
        }
    }
}
