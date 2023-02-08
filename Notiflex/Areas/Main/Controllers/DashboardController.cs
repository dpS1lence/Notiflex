using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notiflex.Core.Models.DTOs;
using Notiflex.ViewModels;
using Notiflex.Core.Services.AccountServices;
using Notiflex.Core.Services.BotServices;
using Notiflex.Core.Services.BOtServices;
using Notiflex.Core.Services.Contracts;
using Notiflex.Core.Services.SchedulerServices;
using System.Security.Claims;
using Telegram.Bot.Types;
using AutoMapper;
using Notiflex.Core.Exceptions;
using Humanizer;
using Quartz;

namespace Notiflex.Areas.Main.Controllers
{
    [Authorize(Roles = "ApprovedUser")]
    [Area("Main")]

    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMessageSender _messageSender;
        private readonly IMessageConfigurer _messageConfigurer;
        private readonly IAccountService _accountService;
        private readonly IModelConfigurer _modelConfigurer;
        private readonly IWeatherApiService _weatherApiService;
        private readonly ITriggerService _triggerService;
        private readonly IMapper _mapper;

        public DashboardController(IDashboardService dashboardService,
            IMessageSender messageSender,
            IMessageConfigurer messageConfigurer,
            IAccountService accountService,
            IModelConfigurer modelConfigurer,
            IWeatherApiService weatherApiService,
            ITriggerService triggerService,
            IMapper mapper
        )
        {
            _dashboardService = dashboardService;
            _messageSender = messageSender;
            _messageConfigurer = messageConfigurer;
            _accountService = accountService;
            _modelConfigurer = modelConfigurer;
            _weatherApiService = weatherApiService;
            _triggerService = triggerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Weather()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new NotFoundException("UserId null.");
            }

            var profileData = await _accountService.GetUserData(userId ?? string.Empty);
            var dto = await _modelConfigurer.ConfigureForecastReport(profileData.HomeTown ?? string.Empty);
            List<WeatherCardViewModel> model = _mapper.Map<List<WeatherCardViewModel>>(dto);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Weather(string value)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new ArgumentException("UserId null.");
            }
            List<WeatherCardViewModel> model = _mapper.Map<List<WeatherCardViewModel>>(await _modelConfigurer.ConfigureForecastReport(value ?? string.Empty));
            return View(model);
        }

        [HttpGet]
        public IActionResult Triggers()
        {
            var model = new List<TriggerAddViewModel>()
                {
                    new TriggerAddViewModel()
                    {
                        Id = 2,
                        Name = "Trigger Name",
                        City = "Varna",
                        Hour = 8,
                        Minutes = "00",
                        Meridiem = "PM",
                        DaySchedule = new Dictionary<DayOfWeek, bool>
                        {
                            { DayOfWeek.Monday, true },
                            { DayOfWeek.Saturday, true }
                        }
                    },
                    new TriggerAddViewModel()
                    {
                        Id = 1,
                        Name = "School",
                        City = "Veliko Tarnovo",
                        Hour = 7,
                        Minutes = "00",
                        Meridiem = "AM",
                        DaySchedule = new Dictionary<DayOfWeek, bool>
                        {
                            { DayOfWeek.Monday, true },
                            { DayOfWeek.Tuesday, true }
                        }
                    }
                };
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateTrigger()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrigger(TriggerAddViewModel model)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _accountService.GetUserData(userId);

            DayOfWeek[] daysSchedule = model.DaySchedule.Where(a => a.Value == true).Select(a => a.Key).ToArray();
            await _triggerService.CreateWeatherReportTriggerAsync(userId, model.Name,  model.City, user.TelegramChatId, new TimeOfDay(model.Hour - 2, int.Parse(model.Minutes)), daysSchedule);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new NotFoundException("UserId null.");
            }

            var dashboardData = await _dashboardService.LoadDashboardAsync(userId);

            var weatherCard = _mapper.Map<List<WeatherCardViewModel>>(dashboardData.DashboardWeatherCard);
            var profileModel = _mapper.Map<ProfileViewModel>(dashboardData.ProfileView);

            var model = new DashboardViewModel()
            {
                DashboardWeatherCard = weatherCard,
                ProfileView = profileModel,
                TimeRanges = dashboardData.TimeRanges,
                TempData = dashboardData.TempData,
                CloudsData = dashboardData.CloudsData,
                PressureData = dashboardData.PressureData
            };

            return View(model);
        }

        public async Task<IActionResult> DeleteTrigger(int triggerId)
        {
            //TODO
            //Check if the given triggerId belongs to the user
            //If so, delete it

            return RedirectToAction(nameof(Dashboard));
        }
    }
}
