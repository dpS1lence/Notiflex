﻿using Microsoft.AspNetCore.Identity;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Services.BotServices;
using Notiflex.Core.Services.Contracts;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.Infrastructure.Repositories.Contracts;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.AccountServices
{
    public class DashboardService : IDashboardService
    {
        private readonly IModelConfigurer _modelConfigurer;
        private readonly IAccountService _accountService;

        public DashboardService(IModelConfigurer modelConfigurer, IAccountService accountService)
        {
            _modelConfigurer = modelConfigurer;
            _accountService = accountService;
        }


        public async Task<DashboardDto> LoadDashboardAsync(string userId)
        {
            var profileData = await _accountService.GetUserData(userId ?? string.Empty);
            var weatherCard = await _modelConfigurer.ConfigureForecastReport(profileData.HomeTown ?? string.Empty);

            List<string> timesList = new();
            for (var i = 0; i < 7; i++)
            {
                timesList.Add(weatherCard[i].Date.Substring(10, 6));
            }

            List<string> temperaturesList = new();
            for (var i = 0; i < 7; i++)
            {
                temperaturesList.Add(weatherCard[i].Temp[..^1]);
            }

            List<string> cloudDataList = new();
            for (var i = 0; i < 7; i++)
            {
                cloudDataList.Add(weatherCard[i].Clouds);
            }

            List<string> pressureDataList = new();
            for (var i = 0; i < 7; i++)
            {
                pressureDataList.Add(weatherCard[i].Pressure);
            }

            List<string> humidityDataList = new();
            for (var i = 0; i < 7; i++)
            {
                humidityDataList.Add(weatherCard[i].Humidity);
            }

            var model = new DashboardDto()
            {
                DashboardWeatherCard = weatherCard,
                ProfileView = new ProfileDto()
                {
                    FirstName = profileData.FirstName
                },
                TimeRanges = timesList,
                TempData = temperaturesList,
                CloudsData = cloudDataList,
                PressureData = pressureDataList,
                HumidityData = humidityDataList
            };

            return model;
        }
    }
}
