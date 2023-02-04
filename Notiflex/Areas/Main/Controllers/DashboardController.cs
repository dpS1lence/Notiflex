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

namespace Notiflex.Areas.Main.Controllers
{
    [Area("Main")]
    [Authorize]
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
            //BECAUSE WEATHER API DECIDED TO TROLL
            //var profileData = await _dashboardService.GetUserData(userId ?? string.Empty);
            //var dto = await _modelConfigurer.ConfigureForecastReport(profileData.HomeTown ?? string.Empty);
            //List<WeatherCardViewModel> model = _mapper.Map<List<WeatherCardViewModel>>(dto);
            List<WeatherCardViewModel> model = new List<WeatherCardViewModel>();
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
        public async Task<IActionResult> Triggers()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateTrigger()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTrigger(TriggerAddViewModel model)
        {
            return View();
        }

    }
}
