using Notiflex.Core.Models.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.Contracts
{
    public interface IWeatherApiService
    {
        Task<WeatherDataModel> GetDataAsync(string url);
        Task<NameToCoordinatesModel> ConvertFromNameAsync(string url);
    }
}
