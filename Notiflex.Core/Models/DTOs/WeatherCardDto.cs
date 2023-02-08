using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Models.DTOs
{
    public class WeatherCardDto
    {
        public bool Avalable { get; set; } = false;

        public string Weather { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string Temp { get; set; } = null!;

        public string FeelsLike { get; set; } = null!;

        public string TempMin { get; set; } = null!;

        public string TempMax { get; set; } = null!;

        public string Pressure { get; set; } = null!;

        public string Humidity { get; set; } = null!;

        public string Speed { get; set; } = null!;

        public string Date { get; set; } = null!;

        public string Clouds { get; set; } = null!;

        public int TimeZone { get; set; }
    }
}
