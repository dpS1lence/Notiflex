using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notiflex.Core.Models;
using Notiflex.Core.Services.Contracts;
using Notiflex.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Telegram.Bot.Types;

namespace Notiflex.Areas.Home.Controllers
{
    public class HomeController : BaseHomeController
    {        
       
        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Dashboard", "Dashboard", new { area = "Main" });
            }

            return View();
        }
    }
}