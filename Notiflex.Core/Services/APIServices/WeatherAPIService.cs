﻿using Newtonsoft.Json;
using Notiflex.Core.Models.APIModels;
using Notiflex.Core.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.APIServices
{
    public class WeatherAPIService : IWeatherApiService
    {
        public async Task<WeatherDataModel> GetDataAsync(string url)
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);
            if (string.IsNullOrEmpty(json))
            {
                //TODO: New Specific Exception Type
                throw new NullReferenceException();
            }
            var result = JsonConvert.DeserializeObject<WeatherDataModel>(json);
            if (result == null)
            {
                throw new NullReferenceException();
            }
            return result;
        }
        public async Task<List<WeatherDataModel>> GetForecastDataAsync(string url)
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);
            if (string.IsNullOrEmpty(json))
            {
                //TODO: New Specific Exception Type
                throw new NullReferenceException();
            }
            var result = JsonConvert.DeserializeObject<List<WeatherDataModel>>(json);
            if (result == null)
            {
                throw new NullReferenceException();
            }
            return result;
        }

        public async Task<NameToCoordinatesModel> ConvertFromNameAsync(string url)
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);
            if (string.IsNullOrEmpty(json))
            {
                //TODO: New Specific Exception Type
                throw new NullReferenceException();
            }
            var result = JsonConvert.DeserializeObject<List<NameToCoordinatesModel>>(json);
            if (result == null)
            {
                throw new NullReferenceException();
            }
            return result.First();
        }
    }
}
