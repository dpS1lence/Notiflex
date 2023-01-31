using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notiflex.Core.Services.Contracts;
using Notiflex.ViewModels;
using System.Security.Claims;

namespace Notiflex.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

            var dto = await dashboardService.GetUserData(userId);

            return View(new ProfileViewModel()
            {
                ProfilePic = dto.ProfilePic,
                FirstName = dto.FirstName
            });
        }
    }
}
