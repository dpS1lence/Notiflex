using Notiflex.Core.Models.HomePageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.Contracts
{
    public interface IModelConfigurer
    {
        Task<DashboardWeatherCardViewModel> ConfigureWeatherReport(string name);

        Task<List<DashboardWeatherCardViewModel>> ConfigureForecastReport(string name);

        Task<List<string>> ConvertNameToCoordinates(string cityName);
    }
}
