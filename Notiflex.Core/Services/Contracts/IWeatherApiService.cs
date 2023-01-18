using Notiflex.Core.Models.APIModels;
using Notiflex.Core.Models.ForecastApiModel;
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

        Task<ForecastDataModel> GetForecastDataAsync(string url);

        Task<NameToCoordinatesModel> ConvertFromNameAsync(string url);
    }
}
