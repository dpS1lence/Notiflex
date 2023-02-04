using Notiflex.Core.Services.Contracts;
using Notiflex.Infrastructure.Data.Models.ScheduleModels;
using Notiflex.Infrastructure.Repositories.Contracts;
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
        private readonly IRepository _repository;
        
               
        public TriggerService(ISchedulerFactory schedulerFactory, IRepository repository)
        {
            _schedulerFactory = schedulerFactory;
            _repository = repository;
        }

        public async Task CreateWeatherReportTriggerAsync(string userId, string city, string telegramChatId, int interval, IntervalUnit intervalUnit, params DayOfWeek[] daysOfWeek)
        {
            string identity = telegramChatId + " Report Trigger";
            if (intervalUnit == IntervalUnit.Millisecond || intervalUnit == IntervalUnit.Second)
            {
                throw new ArgumentException("Interval not supported.");
            }
            
            var trigger = TriggerBuilder.Create()
                .WithIdentity(identity)
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
            if (triggers.Any(a => a.Key.Name.ToString() == (identity)))
            {
                await scheduler.UnscheduleJob(trigger.Key);
            }
            NotiflexTrigger notiflexTrigger = new NotiflexTrigger()
            {
                Identity = identity,
                City = city,
                Interval = interval,
                IntervalUnit = intervalUnit,
                UserId = userId
            };
            await _repository.AddAsync(notiflexTrigger);
            await _repository.SaveChangesAsync();
            await scheduler.ScheduleJob(trigger);
        }        
    }
}
