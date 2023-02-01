using Microsoft.AspNetCore.Identity;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Services.Contracts;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.Infrastructure.Repositories.Contracts;
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

        public DashboardService(IRepository repo, UserManager<NotiflexUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
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
    }
}
