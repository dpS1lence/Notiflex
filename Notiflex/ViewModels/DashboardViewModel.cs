using Notiflex.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.ViewModels
{
    public class DashboardViewModel
    {
        public List<WeatherCardViewModel>? DashboardWeatherCard { get; set; }

        public ProfileViewModel? ProfileView { get; set; }

        public List<TriggerAddViewModel>? TriggerVIew { get; set; }
    }
}
