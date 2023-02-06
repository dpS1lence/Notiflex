using Microsoft.AspNetCore.Identity;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Services.BOtServices;
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
        private readonly IRepository _repo;
        private readonly UserManager<NotiflexUser> _userManager;
        private readonly IModelConfigurer _modelConfigurer;

        public DashboardService(IRepository repo, UserManager<NotiflexUser> userManager, IModelConfigurer modelConfigurer)
        {
            _repo = repo;
            _userManager = userManager;
            _modelConfigurer = modelConfigurer;
        }

        public async Task<ProfileDto> GetUserData(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var profileDto = new ProfileDto()
            {
                ProfilePic = user.ProfilePic,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DefaultTime= user.DefaultTime,
                Description = user.Description,
                HomeTown= user.HomeTown,
                TelegramChatId = user.TelegramInfo
            };

            return profileDto;
        }

        public async Task<DashboardDto> LoadDashboardAsync(string userId)
        {
            var profileData = await GetUserData(userId ?? string.Empty);
            var weatherCard = await _modelConfigurer.ConfigureForecastReport(profileData.HomeTown ?? string.Empty);

            List<string> timesList = new();
            for (int i = 0; i < 7; i++)
            {
                timesList.Add(weatherCard[i].Date.Substring(10, 6));
            }

            List<string> temperaturesList = new();
            for (int i = 0; i < 7; i++)
            {
                temperaturesList.Add(weatherCard[i].Temp[..^1]);
            }

            List<string> cloudDataList = new();
            for (int i = 0; i < 7; i++)
            {
                cloudDataList.Add(weatherCard[i].Clouds);
            }

            List<string> pressureDataList = new();
            for (int i = 0; i < 7; i++)
            {
                pressureDataList.Add(weatherCard[i].Pressure);
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
                PressureData = pressureDataList
            };

            return model;
        }
    }
}
