using Microsoft.Extensions.Configuration;
using Notiflex.Core.Models.APIModels;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Models.ForecastApiModel;
using Notiflex.Core.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Notiflex.Core.Services.BotServices
{
    public class ModelConfigurer : IModelConfigurer
    {
        private readonly IConfiguration _config;
        private readonly IWeatherApiService _weatherService;

        public ModelConfigurer(IConfiguration config, IWeatherApiService weatherService)
        {
            this._config = config;
            this._weatherService = weatherService;
        }

        public async Task<WeatherCardDto> ConfigureWeatherReport(string name)
        {
            var coors = await ConvertNameToCoordinates(name);

            var lat = coors[0];
            var lon = coors[1];

            StringBuilder api = new();
            api.Append(_config.GetValue<string>("WeatherApi"));
            api.Append($"lat={lat}&lon={lon}&appid=");
            api.Append(_config.GetValue<string>("WeatherKey"));

            var model = await _weatherService.GetDataAsync(api.ToString());

            return FillModel(model);
        }

        public async Task<List<WeatherCardDto>> ConfigureForecastReport(string name)
        {
            if (string.IsNullOrEmpty(name) || (await ConvertNameToCoordinates(name))[2] == null)
            {
                throw new ArgumentException("Invalid city name!");
            }

            var coors = await ConvertNameToCoordinates(name);

            var lat = coors[0];
            var lon = coors[1];
            var ctyName = coors[2];

            StringBuilder api = new();
            api.Append(_config.GetValue<string>("ForecastApi"));
            api.Append($"lat={lat}&lon={lon}&appid=");
            api.Append(_config.GetValue<string>("WeatherKey"));

            var modelList = await _weatherService.GetForecastDataAsync(api.ToString());

            return modelList.List.Select(model => new WeatherCardDto()
                {
                    Avalable = true
                    , Name = ctyName
                    , Country = modelList.City.Country
                    , Weather = model.Weather.First().Main
                    , Description = model.Weather.First().Description
                    , Temp = Math.Round((decimal) (model.Main.Temp - 273.15), 2)
                        .ToString(CultureInfo.InvariantCulture) + "°"
                    , FeelsLike = Math.Round((decimal) (model.Main.FeelsLike - 273.15), 2).ToString(CultureInfo.InvariantCulture) + "°"
                    , TempMin = Math.Round((decimal) (model.Main.TempMin - 273.15), 2).ToString(CultureInfo.InvariantCulture) + "°"
                    , TempMax = Math.Round((decimal) (model.Main.TempMax - 273.15), 2).ToString(CultureInfo.InvariantCulture) + "°"
                    , Pressure = Math.Round((decimal) (model.Main.Pressure), 2)
                        .ToString(CultureInfo.InvariantCulture)
                    , Humidity = Math.Round((decimal) (model.Main.Humidity), 2)
                        .ToString(CultureInfo.InvariantCulture)
                    , Speed = Math.Round((decimal) (model.Wind.Speed), 2)
                        .ToString(CultureInfo.InvariantCulture)
                    , Date = model.DtTxt.ToString()
                    , Clouds = model.Clouds.All.ToString()
                    , Icon = model.Weather.First().Icon
                })
                .ToList();
        }

        public async Task<List<string>> ConvertNameToCoordinates(string cityName)
        {
            StringBuilder api = new();
            api.Append(_config.GetValue<string>("CityNameConverterUrl"));
            api.Append($"{cityName}&limit=1&appid=");
            api.Append(_config.GetValue<string>("WeatherKey"));

            var model = await _weatherService.ConvertFromNameAsync(api.ToString());

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
                Temp = Math.Round((decimal)(model.Main.Temp - 273.15), 2).ToString(CultureInfo.InvariantCulture) + "°",
                FeelsLike = Math.Round((decimal)(model.Main.FeelsLike - 273.15), 2).ToString(CultureInfo.InvariantCulture) + "°",
                TempMin = Math.Round((decimal)(model.Main.TempMin - 273.15), 2).ToString(CultureInfo.InvariantCulture) + "°",
                TempMax = Math.Round((decimal)(model.Main.TempMax - 273.15), 2).ToString(CultureInfo.InvariantCulture) + "°",
                Pressure = Math.Round((decimal)(model.Main.Pressure), 2).ToString(CultureInfo.InvariantCulture),
                Humidity = Math.Round((decimal)(model.Main.Humidity), 2).ToString(CultureInfo.InvariantCulture),
                Speed = Math.Round((decimal)(model.Wind.Speed), 2).ToString(CultureInfo.InvariantCulture),
                TimeZone = model.Timezone,
                Icon = model.Weather.First().Icon
            };

            return indexModel;
        }
    }
}
