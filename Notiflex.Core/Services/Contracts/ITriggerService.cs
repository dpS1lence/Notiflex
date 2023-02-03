using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.Contracts
{
    public interface ITriggerService
    {
        Task CreateWeatherReportTriggerAsync(string city, string telegramChatId, TimeSpan interval);
    }
}
