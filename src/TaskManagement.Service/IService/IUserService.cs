using TaskManagement.Core.DTOs;
using TaskManagement.Core.Models;

namespace TaskManagement.Services.IService
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetUserList();
    }
}
