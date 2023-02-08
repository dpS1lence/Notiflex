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

        public async Task CreateWeatherReportTriggerAsync(string userId,string triggerName,  string city, string telegramChatId, TimeOfDay startingTime, DayOfWeek[] daysOfWeek)
        {
            string identity = telegramChatId + " Report Trigger";           
            
            var trigger = TriggerBuilder.Create()
                .WithIdentity(identity)
                .ForJob("ReportSenderJob")
                .UsingJobData("city", city)
                .UsingJobData("telegramChatId", telegramChatId)   
                .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(startingTime.Hour, startingTime.Minute, daysOfWeek).InTimeZone(TimeZoneInfo.Utc))                
                .Build();
            var scheduler = await _schedulerFactory.GetScheduler();
            var triggers = (await scheduler.GetTriggersOfJob(new JobKey("ReportSenderJob"))).ToList();
            if (triggers.Any(a => a.Key.Name.ToString() == (identity)))
            {
                await scheduler.UnscheduleJob(trigger.Key);
            }
            NotiflexTrigger notiflexTrigger = new NotiflexTrigger()
            {
                Name = triggerName,
                Identity = identity,
                City = city,
                Hour = startingTime.Hour,
                Minutes = startingTime.Minute.ToString(),
                UserId = userId
            };
            await _repository.AddAsync(notiflexTrigger);
            await _repository.SaveChangesAsync();
            await scheduler.ScheduleJob(trigger);
        }        
    }
}
