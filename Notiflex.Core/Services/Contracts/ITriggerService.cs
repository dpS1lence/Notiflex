using Notiflex.Core.Models.DTOs;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.Contracts
{
    public interface ITriggerService
    {
        Task CreateWeatherReportTriggerAsync(string userId, string triggerName, string city, string telegramChatId, TimeOfDay startingTime, DayOfWeek[] daysOfWeek);

        Task<int> GetHourUtc(string cityName, int hour);

        Task<List<TriggerGetOneDto>> GetAllTriggers(string userId);
        Task DeleteTrigger(int triggerId, string userId);
    }
}
