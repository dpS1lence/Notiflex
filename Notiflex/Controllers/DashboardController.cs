using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notiflex.Core.Models.DashboardModels;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Models.HomePageModels;
using Notiflex.Core.Services.AccountServices;
using Notiflex.Core.Services.BotServices;
using Notiflex.Core.Services.BOtServices;
using Notiflex.Core.Services.Contracts;
using Notiflex.Core.Services.SchedulerServices;
using Notiflex.ViewModels;
using System.Security.Claims;
using Telegram.Bot.Types;

namespace Notiflex.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMessageSender _messageSender;
        private readonly IMessageConfigurer _messageConfigurer;
        private readonly IAccountService _accountService;
        private readonly IModelConfigurer _modelConfigurer;
        private readonly IWeatherApiService _weatherApiService;
        private readonly ITriggerService _triggerService;

        public DashboardController(IDashboardService dashboardService, IMessageSender messageSender, IMessageConfigurer messageConfigurer, IAccountService accountService, IModelConfigurer modelConfigurer, IWeatherApiService weatherApiService, ITriggerService triggerService)
        {
            _dashboardService = dashboardService;
            _messageSender = messageSender;
            _messageConfigurer = messageConfigurer;
            _accountService = accountService;
            _modelConfigurer = modelConfigurer;
            _weatherApiService = weatherApiService;
            _triggerService = triggerService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            //await _triggerService.CreateWeatherReportTriggerAsync("Varna", "5184263976", 30);

            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new ArgumentException("UserId null.");
            }

            var profileData = await _dashboardService.GetUserData(userId ?? string.Empty);

            return View(await CreateDashboardViewModel(profileData.HomeTown ?? string.Empty, profileData, userId ?? string.Empty));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Dashboard(string value)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new ArgumentException("UserId null.");
            }

            var profileData = await _dashboardService.GetUserData(userId ?? string.Empty);
            string chatId = (await _accountService.GetUserData(userId)).TelegramInfo ?? throw new ArgumentException("TelegramInfo null.");
            //await _triggerService.CreateWeatherReportTriggerAsync(value, chatId, 30);
            return View(await CreateDashboardViewModel(value ?? string.Empty, profileData, userId ?? string.Empty));
        }

        private async Task<DashbardViewModel> CreateDashboardViewModel(string townName, ProfileDto profileData, string userId)
        {
            var nameConfig = (await _messageConfigurer.ConvertNameToCoordinates(townName ?? string.Empty))[2];

            List<DashboardWeatherCardViewModel> model = await _modelConfigurer.ConfigureForecastReport(townName ?? string.Empty);

            if (User?.Identity?.IsAuthenticated ?? false)
            {
                string chatId = (await _accountService.GetUserData(userId)).TelegramInfo ?? throw new ArgumentException("TelegramInfo null.");

                if (chatId == null)
                {
                    throw new ArgumentException("ChatId null.");
                }

                Message message = await _messageConfigurer.ConfigureWeatherReportMessage(nameConfig);

                //await _messageSender.SendMessage(message, chatId);
            }

            DashbardViewModel viewModel = new()
            {
                ProfileView = new ProfileViewModel()
                {
                    ProfilePic = profileData.ProfilePic,
                    FirstName = profileData.FirstName
                },
                DashboardWeatherCard = model
            };

            return viewModel;
        }
    }
}
