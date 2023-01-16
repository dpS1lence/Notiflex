using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.Contracts
{
    public interface IMessageSender
    {
        Task<string> SendMessage(string message, string chatId);
        Task<string> ConfigureWeatherReport(string name);
        Task<List<string>> ConvertNameToCoordinates(string cityName);
    }
}
