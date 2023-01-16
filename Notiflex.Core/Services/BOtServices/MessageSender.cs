using Microsoft.Extensions.Configuration;
using Notiflex.Core.Models.APIModels;
using Notiflex.Core.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Notiflex.Core.Services.BotServices
{
    public class MessageSender : IMessageSender
    {
        private readonly IConfiguration config;
        private readonly IWeatherApiService weatherService;

        public MessageSender(IConfiguration config, IWeatherApiService weatherService)
        {
            this.config = config;
            this.weatherService = weatherService;
        }

        public async Task<string> SendMessage(string message, string chatId)
        {
            TelegramBotClient bot = new(config.GetValue<string>("Notiflex_botId"));

            Message msg = await bot.SendTextMessageAsync(chatId, message);

            return msg.Text ?? throw new ArgumentException("Error.");
        }

        public async Task<string> ConfigureWeatherReport(string name)
        {
            List<string> coors = await ConvertNameToCoordinates(name);

            string lat = coors[0];
            string lon = coors[1];

            StringBuilder api = new();
            api.Append(config.GetValue<string>("WeatherApi"));
            api.Append($"lat={lat}&lon={lon}&appid=");
            api.Append(config.GetValue<string>("WeatherKey"));

            WeatherDataModel model = await weatherService.GetDataAsync(api.ToString());

            StringBuilder message = new();
            message.AppendLine($"Today's weather report for {model.Name} ({model.Sys.Country})");
            message.AppendLine($"temp -> {(model.Main.Temp - 273.15):f2}");
            message.AppendLine($"feels_like -> {(model.Main.FeelsLike - 273.15):f2}");
            message.AppendLine($"temp_min -> {(model.Main.TempMin - 273.15):f2}");
            message.AppendLine($"temp_max -> {(model.Main.TempMax - 273.15):f2}");
            message.AppendLine($"pressure -> {model.Main.Pressure}");
            message.AppendLine($"humidity -> {model.Main.Humidity}");
            message.AppendLine($"wind speed -> {model.Wind.Speed}");

            return message.ToString();
        }

        public async Task<List<string>> ConvertNameToCoordinates(string cityName)
        {
            StringBuilder api = new();
            api.Append(config.GetValue<string>("CityNameConverterUrl"));
            api.Append($"{cityName}&limit=1&appid=");
            api.Append(config.GetValue<string>("WeatherKey"));

            NameToCoordinatesModel model = await weatherService.ConvertFromNameAsync(api.ToString());

            return new List<string>()
            {
                $"{model.Lat}",
                $"{model.Lon}"
            };
        }
    }
}
