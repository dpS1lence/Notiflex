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
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Dashboard", "Dashboard", new { area = "Main" });
            }

            return View();
        }

		public IActionResult Telegram()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}