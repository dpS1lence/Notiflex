using Newtonsoft.Json;
using Notiflex.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services
{
    public class WeatherAPIService : IWeatherApiService
    {
        public async Task<IWeatherApiService> GetDataAsync(string url)
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);
            if (string.IsNullOrEmpty(json))
            {
                //TODO: New Specific Exception Type
                throw new NullReferenceException();
            }
            var result = JsonConvert.DeserializeObject<WeatherAPIService>(json);
            if (result == null)
            {
                throw new NullReferenceException();
            }
            return result;
        }
    }
}
