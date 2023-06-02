using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
