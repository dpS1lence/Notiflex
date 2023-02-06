using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Models.DTOs
{
    public class DashboardDto
    {
        public List<WeatherCardDto>? DashboardWeatherCard { get; set; }

        public ProfileDto? ProfileView { get; set; }

        public List<string> TimeRanges { get; set; } = null!;

        public List<string> TempData { get; set; } = null!;

        public List<string> CloudsData { get; set; } = null!;

        public List<string> PressureData { get; set; } = null!;
    }
}
