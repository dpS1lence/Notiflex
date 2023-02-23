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

namespace Notiflex.Core.Services.BotServices
{
    public class MessageConfigurer : IMessageConfigurer
    {
        private readonly IConfiguration _config;
        private readonly IWeatherApiService _weatherService;

        public MessageConfigurer(IConfiguration config, IWeatherApiService weatherService)
        {
            this._config = config;
            this._weatherService = weatherService;
        }

        public async Task<WeatherReportReturnType> ConfigureWeatherReportWithFile(string name)
        {
            var coors = await ConvertNameToCoordinates(name);

            var message = string.Empty;
            var location = new Location();

            if (coors.Count != 0)
            {
                location.Latitude = double.Parse(coors[0]);
                location.Longitude = double.Parse(coors[1]);

                StringBuilder api = new();
                api.Append(_config.GetValue<string>("WeatherApi"));
                api.Append($"lat={location.Latitude}&lon={location.Longitude}&appid=");
                api.Append(_config.GetValue<string>("WeatherKey"));

                var model = await _weatherService.GetDataAsync(api.ToString());

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
            var coors = await ConvertNameToCoordinates(name);

            var message = string.Empty;
            var location = new Location();

            if (coors.Count != 0)
            {
                location.Latitude = double.Parse(coors[0]);
                location.Longitude = double.Parse(coors[1]);

                StringBuilder api = new();
                api.Append(_config.GetValue<string>("WeatherApi"));
                api.Append($"lat={location.Latitude}&lon={location.Longitude}&appid=");
                api.Append(_config.GetValue<string>("WeatherKey"));

                var model = await _weatherService.GetDataAsync(api.ToString());

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
            api.Append(_config.GetValue<string>("CityNameConverterUrl"));
            api.Append($"{cityName}&limit=1&appid=");
            api.Append(_config.GetValue<string>("WeatherKey"));

            NameToCoordinatesModel model;

            try
            {
                model = await _weatherService.ConvertFromNameAsync(api.ToString());
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
