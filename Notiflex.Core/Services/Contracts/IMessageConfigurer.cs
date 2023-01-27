using Notiflex.Core.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;


namespace Notiflex.Core.Services.Contracts
{
    public interface IMessageConfigurer
    {
        Task<List<string>> ConvertNameToCoordinates(string cityName);

        Task<Message> ConfigureWeatherReportMessage(string name);

        Task<WeatherReportReturnType> ConfigureWeatherReportWithFile(string name);
    }
}
