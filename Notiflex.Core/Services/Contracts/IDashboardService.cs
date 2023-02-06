using Notiflex.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.Contracts
{
    public interface IDashboardService
    {
        Task<ProfileDto> GetUserData(string userId);

        Task<DashboardDto> LoadDashboardAsync(string userId);
    }
}
