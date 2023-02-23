using Newtonsoft.Json;
using Notiflex.Core.Exceptions;
using Notiflex.Core.Models.APIModels;
using Notiflex.Core.Models.ForecastApiModel;
using Notiflex.Core.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.APIServices
{
    public class WeatherApiService : IWeatherApiService
    {
        public async Task<WeatherDataModel> GetDataAsync(string url)
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);

            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentException("NullOrEmpty");
            }

            var result = JsonConvert.DeserializeObject<WeatherDataModel>(json);
            if (result == null)
            {
                throw new NotFoundException();
            }

            return result;
        }
        public async Task<ForecastDataModel> GetForecastDataAsync(string url)
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);

            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentException("NullOrEmpty");
            }

            var result = JsonConvert.DeserializeObject<ForecastDataModel>(json);
            if (result == null)
            {
                throw new ArgumentException("NullOrEmpty");
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
                throw new ArgumentException("NullOrEmpty");
            }

            var result = JsonConvert.DeserializeObject<List<NameToCoordinatesModel>>(json);
            if (result == null)
            {
                throw new ArgumentException("NullOrEmpty");
            }

            return result.Count <= 0 ? new NameToCoordinatesModel() : result.First();
        }
    }
}
