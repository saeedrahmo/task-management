using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Api.Models
{
    public class AuthViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; init; }
    }
}
