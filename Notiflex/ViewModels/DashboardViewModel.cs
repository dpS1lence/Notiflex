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

        public List<string> TimeRanges { get; set; } = null!;

        public List<string> TempData { get; set; } = null!;

        public List<string> CloudsData { get; set; } = null!;

        public List<string> PressureData { get; set; } = null!;
    }
}
