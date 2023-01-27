using Microsoft.AspNetCore.Mvc;

namespace Notiflex.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
