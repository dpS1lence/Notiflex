using Notiflex.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.Contracts
{
    public interface IModelConfigurer
    {
        Task<WeatherCardDto> ConfigureWeatherReport(string name);

        Task<List<WeatherCardDto>> ConfigureForecastReport(string name);

        Task<List<string>> ConvertNameToCoordinates(string cityName);
    }
}
