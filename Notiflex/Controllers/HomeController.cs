using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notiflex.Core.Models;
using Notiflex.Core.Models.HomePageModels;
using Notiflex.Core.Services.Contracts;
using Notiflex.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Telegram.Bot.Types;

namespace Notiflex.Controllers
{
    public class HomeController : Controller
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
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Browse()
        {
            List<IndexModel> model = new()
            {
                new IndexModel()
                {
                    Avalable = false
                }
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> SendData(string id)
        {
            Message message = await _messageConfigurer.ConfigureWeatherReportMessage("Veliko Tarnovo");

            await _messageSender.SendMessage(message, id);

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Browse(string value)
        {
            try
            {
                var nameConfig = (await _messageConfigurer.ConvertNameToCoordinates(value))[2];

                if (nameConfig == null)
                {
                    return BadRequest();
                }

                List<IndexModel> model = await _modelConfigurer.ConfigureForecastReport(value);

                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

                    if (userId == null)
                    {
                        throw new ArgumentException("UserId null.");
                    }

                    string chatId = (await _accountService.GetUserData(userId)).TelegramInfo ?? throw new ArgumentException("TelegramInfo null.");

                    if (chatId == null)
                    {
                        throw new ArgumentException("ChatId null.");
                    }

                    Message message = await _messageConfigurer.ConfigureWeatherReportMessage(nameConfig);

                    await _messageSender.SendMessage(message, chatId);
                }

                return View(model);
            }
            catch (Exception)
            {
                return View();
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}