using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notiflex.Core.Models.DTOs;
using Notiflex.ViewModels;
using Notiflex.Core.Services.AccountServices;
using Notiflex.Core.Services.BotServices;
using Notiflex.Core.Services.BotServices;
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
        private readonly IAccountService _accountService;
        private readonly IModelConfigurer _modelConfigurer;
        private readonly ITriggerService _triggerService;
        private readonly IMapper _mapper;

        public DashboardController(IDashboardService dashboardService,            
            IAccountService accountService,
            IModelConfigurer modelConfigurer,
            ITriggerService triggerService,
            IMapper mapper
        )
        {
            _dashboardService = dashboardService;
            _accountService = accountService;
            _modelConfigurer = modelConfigurer;
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

            var profileData = await _accountService.GetUserData(userId);
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

            var user = await _accountService.GetUserData(userId ?? string.Empty);

            List<WeatherCardViewModel> model = new();

            try
            {
                model = _mapper.Map<List<WeatherCardViewModel>>(await _modelConfigurer.ConfigureForecastReport(value ?? string.Empty));
            }
            catch(ArgumentException ex)
            {
                model = _mapper.Map<List<WeatherCardViewModel>>(await _modelConfigurer.ConfigureForecastReport(user.HomeTown));

                TempData["StatusMessageDanger"] = ex.Message;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Triggers()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value!;

            var model = _mapper.Map<List<TriggerGetOneViewModel>>(await _triggerService.GetAllTriggers(userId));

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
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value!;
            var user = await _accountService.GetUserData(userId);

            int hourUTC;
            if (model.GivenTimeZone)
            {
                hourUTC = await _triggerService.GetHourUTC(model.City, model.Hour);

            }
            else
            {
                hourUTC = await _triggerService.GetHourUTC(user.HomeTown, model.Hour);
            }

            if(model.DaySchedule.Select(a => a.Value).Where(a => a == true).ToList().Count <= 0)
            {
                TempData["StatusMessageDanger"] = "Invalid Data!";

                return RedirectToAction(nameof(CreateTrigger));
            }
            
            DayOfWeek[] daysSchedule = model.DaySchedule.Where(a => a.Value).Select(a => a.Key).ToArray();
            
            try
            {
                await _triggerService.CreateWeatherReportTriggerAsync(userId, model.Name, model.City, user.TelegramChatId, new TimeOfDay(hourUTC, int.Parse(model.Minutes)), daysSchedule);
            }
            catch (ArgumentException ex)
            {
                TempData["StatusMessageDanger"] = ex.Message;

                return RedirectToAction(nameof(CreateTrigger));
            }
            
            return RedirectToAction("Triggers", "Dashboard", "Main");
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
                PressureData = dashboardData.PressureData,
                HumidityData = dashboardData.HumidityData
            };

            return View(model);
        }

        public async Task<IActionResult> DeleteTrigger(int triggerId)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value!;

            await _triggerService.DeleteTrigger(triggerId, userId);

            return RedirectToAction(nameof(Triggers));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value!;

            ProfileDto user = await _accountService.GetUserData(userId);

            var model = _mapper.Map<ProfileViewModel>(user);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if ((await _modelConfigurer.ConvertNameToCoordinates(model.HomeTown))[2] == null)
            {
                TempData["StatusMessageDanger"] = "Invalid city name!";

                return RedirectToAction(nameof(Profile));
            }

            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value!;

            var dto = new ProfileDto()
            {
                TelegramChatId = model.TelegramChatId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Description = model.Description,
                ProfilePic = model.ProfilePic,
                HomeTown = model.HomeTown
            };

            await _accountService.EditProfile(userId, dto);
            
            return RedirectToAction(nameof(Profile));
        }
    }
}
