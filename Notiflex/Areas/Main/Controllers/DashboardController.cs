﻿using Microsoft.AspNetCore.Authorization;
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

            var profileData = await _dashboardService.GetUserData(userId ?? string.Empty);
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
        public async Task<IActionResult> Triggers()
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
            DayOfWeek[] daysSchedule = model.DaySchedule.Where(a => a.Value == true).Select(a => a.Key).ToArray();

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

            var profileData = await _dashboardService.GetUserData(userId ?? string.Empty);
            var dto = await _modelConfigurer.ConfigureForecastReport(profileData.HomeTown ?? string.Empty);
            List<WeatherCardViewModel> weatherCard = _mapper.Map<List<WeatherCardViewModel>>(dto);
            List<string> timesList = new();
            for (int i = 0;i < 7;i++)
            {
                timesList.Add(dto[i].Date.Substring(10, 6));
            }

            List<string> temperaturesList = new();
            for (int i = 0; i < 7; i++)
            {
                temperaturesList.Add(dto[i].Temp[..^1]);
            }

            List<string> cloudDataList = new();
            for (int i = 0; i < 7; i++)
            {
                cloudDataList.Add(dto[i].Clouds);
            }

            List<string> pressureDataList = new();
            for (int i = 0; i < 7; i++)
            {
                pressureDataList.Add(dto[i].Pressure);
            }

            var model = new DashboardViewModel()
            {
                DashboardWeatherCard = weatherCard,
                ProfileView = new ProfileViewModel()
                {
                    FirstName = profileData.FirstName
                },
                TimeRanges = timesList,
                TempData = temperaturesList,
                CloudsData = cloudDataList,
                PressureData = pressureDataList
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