using Microsoft.Extensions.Configuration;
using Notiflex.Core.Models.APIModels;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Models.ForecastApiModel;
using Notiflex.Core.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Notiflex.Core.Services.BOtServices
{
    public class ModelConfigurer : IModelConfigurer
    {
        private readonly IConfiguration config;
        private readonly IWeatherApiService weatherService;

        public ModelConfigurer(IConfiguration config, IWeatherApiService weatherService)
        {
            this.config = config;
            this.weatherService = weatherService;
        }

        public async Task<WeatherCardDto> ConfigureWeatherReport(string name)
        {
            List<string> coors = await ConvertNameToCoordinates(name);

            string lat = coors[0];
            string lon = coors[1];

            StringBuilder api = new();
            api.Append(config.GetValue<string>("WeatherApi"));
            api.Append($"lat={lat}&lon={lon}&appid=");
            api.Append(config.GetValue<string>("WeatherKey"));

            WeatherDataModel model = await weatherService.GetDataAsync(api.ToString());

            return FillModel(model);
        }

        public async Task<List<WeatherCardDto>> ConfigureForecastReport(string name)
        {
            if (string.IsNullOrEmpty(name) || (await ConvertNameToCoordinates(name))[2] == null)
            {
                throw new ArgumentException("Invalid city name!");
            }

            List<string> coors = await ConvertNameToCoordinates(name);

            string lat = coors[0];
            string lon = coors[1];
            string ctyName = coors[2];

            StringBuilder api = new();
            api.Append(config.GetValue<string>("ForecastApi"));
            api.Append($"lat={lat}&lon={lon}&appid=");
            api.Append(config.GetValue<string>("WeatherKey"));

            ForecastDataModel modelList = await weatherService.GetForecastDataAsync(api.ToString());
            List<WeatherCardDto> indexModels = new();

            foreach (var model in modelList.List)
            {
                indexModels.Add(new WeatherCardDto()
                {
                    Avalable = true,
                    Name = ctyName,
                    Country = modelList.City.Country,
                    Weather = model.Weather.First().Main,
                    Description = model.Weather.First().Description,
                    Temp = Math.Round((decimal)(model.Main.Temp - 273.15), 2).ToString() + "°",
                    FeelsLike = Math.Round((decimal)(model.Main.FeelsLike - 273.15), 2).ToString() + "°",
                    TempMin = Math.Round((decimal)(model.Main.TempMin - 273.15), 2).ToString() + "°",
                    TempMax = Math.Round((decimal)(model.Main.TempMax - 273.15), 2).ToString() + "°",
                    Pressure = Math.Round((decimal)(model.Main.Pressure), 2).ToString(),
                    Humidity = Math.Round((decimal)(model.Main.Humidity), 2).ToString(),
                    Speed = Math.Round((decimal)(model.Wind.Speed), 2).ToString(),
                    Date = model.DtTxt.ToString(),
                    Clouds = model.Clouds.All.ToString(),
                    Icon = model.Weather.First().Icon
                }) ;
            }

            return indexModels;
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
                $"{model.Lon}",
                model.Name
            };
        }

        private static WeatherCardDto FillModel(WeatherDataModel model)
        {
            WeatherCardDto indexModel = new()
            {
                Avalable = true,
                Name = model.Name,
                Country = model.Sys.Country,
                Weather = model.Weather.First().Main,
                Description = model.Weather.First().Description,
                Temp = Math.Round((decimal)(model.Main.Temp - 273.15), 2).ToString() + "°",
                FeelsLike = Math.Round((decimal)(model.Main.FeelsLike - 273.15), 2).ToString() + "°",
                TempMin = Math.Round((decimal)(model.Main.TempMin - 273.15), 2).ToString() + "°",
                TempMax = Math.Round((decimal)(model.Main.TempMax - 273.15), 2).ToString() + "°",
                Pressure = Math.Round((decimal)(model.Main.Pressure), 2).ToString(),
                Humidity = Math.Round((decimal)(model.Main.Humidity), 2).ToString(),
                Speed = Math.Round((decimal)(model.Wind.Speed), 2).ToString(),
                TimeZone = model.Timezone,
                Icon = model.Weather.First().Icon
            };

            return indexModel;
        }
    }
}
