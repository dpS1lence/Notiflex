using Microsoft.Extensions.Configuration;
using Notiflex.Core.Models.APIModels;
using Notiflex.Core.Services.Contracts;
using Notiflex.Core.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Notiflex.Core.Services.BOtServices
{
    public class MessageConfigurer : IMessageConfigurer
    {
        private readonly IConfiguration config;
        private readonly IWeatherApiService weatherService;

        public MessageConfigurer(IConfiguration config, IWeatherApiService weatherService)
        {
            this.config = config;
            this.weatherService = weatherService;
        }

        public async Task<WeatherReportReturnType> ConfigureWeatherReportWithFile(string name)
        {
            List<string> coors = await ConvertNameToCoordinates(name);

            string message = string.Empty;
            var location = new Location();

            if (coors.Count != 0)
            {
                location.Latitude = double.Parse(coors[0]);
                location.Longitude = double.Parse(coors[1]);

                StringBuilder api = new();
                api.Append(config.GetValue<string>("WeatherApi"));
                api.Append($"lat={location.Latitude}&lon={location.Longitude}&appid=");
                api.Append(config.GetValue<string>("WeatherKey"));

                WeatherDataModel model = await weatherService.GetDataAsync(api.ToString());

                message = ConfigureMessageText(model, name);
            }

            var msg = new Message()
            {
                Text = message.ToString(),
                Location = location
            };

            var file = new FileStream("wwwroot/images/vectorLandScape.jpg", FileMode.Open);

            return new WeatherReportReturnType(msg, file);
        }

        public async Task<Message> ConfigureWeatherReportMessage(string name)
        {
            List<string> coors = await ConvertNameToCoordinates(name);

            string message = string.Empty;
            var location = new Location();

            if (coors.Count != 0)
            {
                location.Latitude = double.Parse(coors[0]);
                location.Longitude = double.Parse(coors[1]);

                StringBuilder api = new();
                api.Append(config.GetValue<string>("WeatherApi"));
                api.Append($"lat={location.Latitude}&lon={location.Longitude}&appid=");
                api.Append(config.GetValue<string>("WeatherKey"));

                WeatherDataModel model = await weatherService.GetDataAsync(api.ToString());

                message = ConfigureMessageText(model, name);
            }

            var msg = new Message()
            {
                Text = message.ToString(),
                Location = location
            };

            return msg;
        }

        public async Task<List<string>> ConvertNameToCoordinates(string cityName)
        {
            StringBuilder api = new();
            api.Append(config.GetValue<string>("CityNameConverterUrl"));
            api.Append($"{cityName}&limit=1&appid=");
            api.Append(config.GetValue<string>("WeatherKey"));

            NameToCoordinatesModel model;

            try
            {
                model = await weatherService.ConvertFromNameAsync(api.ToString());
            }
            catch (Exception)
            {
                return new List<string>();
            }

            return new List<string>()
            {
                $"{model.Lat}",
                $"{model.Lon}",
                model.Name
            };
        }

        private static string ConfigureMessageText(WeatherDataModel model, string cityName)
        {
            StringBuilder message = new();

            message.AppendLine($"🌤️ Weather Report for {cityName}({model.Sys.Country}) - Your One-Stop Shop for All Things Weather and Style!");
            message.AppendLine();
            //message.AppendLine($"📅 {DateTime.FromFileTime(model.Dt)} - Time to Check the Skies and Your Closet!");
            message.AppendLine($"🌦️ Current Conditions: {model.Weather.First().Description} - How's the Weather Outside?");
            message.AppendLine();
            message.AppendLine($"🌡️ Temperature: {(model.Main.Temp - 273.15):f2}°C - How's the Heat?");
            message.AppendLine($"🥶 Min Temperature: {(model.Main.TempMin - 273.15):f2}°C");
            message.AppendLine($"🔥 Max Temperature: {(model.Main.TempMax - 273.15):f2}°C");
            message.AppendLine();
            message.AppendLine($"💨 Wind Speed: {model.Wind.Speed} mph - Windy Today?");
            message.AppendLine($"🌬️ Wind Direction: {model.Wind.Deg} - Which Way is the Wind Blowing?");
            message.AppendLine();
            message.AppendLine($"🌳 Pressure: {model.Main.Pressure} hPa");
            message.AppendLine($"🌬️ Humidity: {model.Main.Humidity}%");          

            return message.ToString();
        }
    }
}
