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
        private readonly IMessageSender _messageSender;
        private readonly IMessageConfigurer _messageConfigurer;
        private readonly IAccountService _accountService;
        private readonly IModelConfigurer _modelConfigurer;
        private readonly IWeatherApiService _weatherApiService;

        public HomeController(IMessageSender messageSender, IMessageConfigurer messageConfigurer, IAccountService accountService, IModelConfigurer modelConfigurer, IWeatherApiService weatherApiService)
        {
            _messageSender = messageSender;
            _messageConfigurer = messageConfigurer;
            _accountService = accountService;
            _modelConfigurer = modelConfigurer;
            _weatherApiService = weatherApiService;
        }

        public IActionResult Index()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
            //var kur =  (_accountService.IsInRole(userId, "ApprovedUser")).Result;
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Dashboard", "Dashboard", new { area = "Main" });
            }

            return View();
        }
    }
}