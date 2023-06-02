using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [InverseProperty("User")]
        public ICollection<Task>? Tasks { get; set; }
    }
}
