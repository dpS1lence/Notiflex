using Microsoft.AspNetCore.Mvc;
using Notiflex.Core.Models;
using Notiflex.Core.Models.HomePageModels;
using Notiflex.Core.Services.Contracts;
using System.Diagnostics;
using Telegram.Bot.Types;

namespace Notiflex.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageSender _messageSender;
        private readonly IModelConfigurer _modelConfigurer;

        public HomeController(ILogger<HomeController> logger, IMessageSender messageSender, IModelConfigurer modelConfigurer)
        {
            _logger = logger;
            _messageSender = messageSender;
            _modelConfigurer = modelConfigurer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<IndexModel> model = new()
            {
                new IndexModel(){ Avalable = false }
            };

            return View(model);
        }

        public async Task<IActionResult> Index(string value)
        {
            if ((await _messageSender.ConvertNameToCoordinates(value)) == null)
            {
                return BadRequest();
            }

            List<IndexModel> model = await _modelConfigurer.ConfigureForecastReport(value);

            return View(model);
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