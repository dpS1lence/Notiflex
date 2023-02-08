using Notiflex.Core.Models.DTOs;
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
        private readonly IModelConfigurer _modelConfigurer;

        public TriggerService(ISchedulerFactory schedulerFactory, IRepository repository, IModelConfigurer modelConfigurer)
        {
            _schedulerFactory = schedulerFactory;
            _repository = repository;
            _modelConfigurer = modelConfigurer;
        }

        public async Task CreateWeatherReportTriggerAsync(string userId,string triggerName,  string city, string telegramChatId, TimeOfDay startingTime, DayOfWeek[] daysOfWeek)
        {
            string identity = telegramChatId + " Report Trigger";           
            
            var trigger = TriggerBuilder.Create()
                .WithIdentity(identity)
                .ForJob("ReportSenderJob")
                .UsingJobData("city", city)
                .UsingJobData("telegramChatId", telegramChatId)   
                .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(startingTime.Hour, startingTime.Minute, daysOfWeek)
                .InTimeZone(TimeZoneInfo.Utc))
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
        
        public async Task<int> GetHourUTC(string cityName, int hour)
        {
            if (!(await _modelConfigurer.ConvertNameToCoordinates(cityName)).Any())
                throw new ArgumentException("Invalid city name");

            var report = await _modelConfigurer.ConfigureWeatherReport(cityName);

            int timezone = (report.TimeZone / 60) / 60;

            int hourUTC;

            if (hour + timezone <= 0)
            {
                hourUTC = 24 + (hour + timezone);
            }
            else if(hour + timezone > 24)
            {
                hourUTC = (hour + timezone) - 24;
            }
            else hourUTC = hour + timezone;

            if(hourUTC == 24)
            {
                hourUTC = 0;
            }

            return hourUTC;
        }
    }
}
