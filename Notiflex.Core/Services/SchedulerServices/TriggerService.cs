using Notiflex.Core.Services.Contracts;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.SchedulerServices
{
    public class TriggerService : ITriggerService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        
               
        public TriggerService(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task CreateWeatherReportTriggerAsync(string city, string telegramChatId, int interval, IntervalUnit intervalUnit, params DayOfWeek[] daysOfWeek)
        {
            
            var trigger = TriggerBuilder.Create()
                .WithIdentity(telegramChatId + " report trigger")
                .ForJob("ReportSenderJob")
                .UsingJobData("city", city)
                .UsingJobData("telegramChatId", telegramChatId)   
                .WithDailyTimeIntervalSchedule(sch => sch.InTimeZone(TimeZoneInfo.Utc)
                                                      .OnDaysOfTheWeek(daysOfWeek)
                                                      .WithInterval(interval, intervalUnit)
                                                      .StartingDailyAt(TimeOfDay.HourMinuteAndSecondOfDay(12, 0, 0)))
                .StartNow()                                                
                .Build();
            var scheduler = await _schedulerFactory.GetScheduler();
            var triggers = (await scheduler.GetTriggersOfJob(new JobKey("ReportSenderJob"))).ToList();
            if (triggers.Any(a => a.Key.Name.ToString() == (telegramChatId + " report trigger")))
            {
                await scheduler.UnscheduleJob(trigger.Key);
            }
            await scheduler.ScheduleJob(trigger);
        }        
    }
}
