using Microsoft.AspNetCore.Mvc;
using Notiflex.Core.Models;
using Notiflex.Core.Services.Contracts;
using Notiflex.ViewModels;
using System.Diagnostics;
using Telegram.Bot.Types;

namespace Notiflex.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageSender _messageSender;

        public HomeController(ILogger<HomeController> logger, IMessageSender messageSender)
        {
            _logger = logger;
            _messageSender = messageSender;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Index(string value)
        {
            if((await _messageSender.ConvertNameToCoordinates(value)) == null)
            {
                return BadRequest();
            }
            string message = await _messageSender.ConfigureWeatherReport(value);

            await _messageSender.SendMessage(message, "5184263976");

            return View();
        }

        public IActionResult Privacy()
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