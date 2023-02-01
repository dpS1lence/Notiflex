using Notiflex.Core.Models.HomePageModels;
using Notiflex.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Models.DashboardModels
{
    public class DashbardViewModel
    {
        public List<DashboardWeatherCardViewModel>? DashboardWeatherCard { get; set; }
        public ProfileViewModel? ProfileView { get; set; }
    }
}
