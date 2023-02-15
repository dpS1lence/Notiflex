using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Notiflex.Core.Exceptions;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Services.Contracts;
using Notiflex.Infrastructure.Data.Models.ScheduleModels;
using Notiflex.Infrastructure.Data.Models.UserModels;
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
        private readonly UserManager<NotiflexUser> _userManager;

        public TriggerService(ISchedulerFactory schedulerFactory, IRepository repository, IModelConfigurer modelConfigurer, UserManager<NotiflexUser> userManager)
        {
            _schedulerFactory = schedulerFactory;
            _repository = repository;
            _modelConfigurer = modelConfigurer;
            _userManager = userManager;
        }

        public async Task CreateWeatherReportTriggerAsync(string userId, string triggerName, string city, string telegramChatId, TimeOfDay startingTime, DayOfWeek[] daysOfWeek)
        {
            if ((await _modelConfigurer.ConvertNameToCoordinates(city))[2] == null)
            {
                throw new ArgumentException("Invalid city name!");
            }

            if(userId == null || telegramChatId == null)
            {
                throw new ArgumentException("Invalid model!");
            }

            string identity = Guid.NewGuid().ToString();

            int minutes;
            if(startingTime.Minute.ToString() == "00")
            {
                minutes = 0;
            }
            else
            {
                minutes = startingTime.Minute;
            }

            var trigger = TriggerBuilder.Create()
                .WithIdentity(identity)
                .ForJob("ReportSenderJob")
                .UsingJobData("city", city)
                .UsingJobData("telegramChatId", telegramChatId)
                .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(startingTime.Hour, minutes, daysOfWeek)
                .InTimeZone(TimeZoneInfo.Utc))
                .Build();

            var scheduler = await _schedulerFactory.GetScheduler();
            string days = string.Empty;
            foreach (var item in daysOfWeek)
            {
                days += item + ", ";
            }
            days = days.Substring(0, days.Length - 2);

            var notiflexTrigger = new NotiflexTrigger()
            {
                Name = triggerName,
                Identity = identity,
                City = city,
                Hour = startingTime.Hour,
                Minutes = startingTime.Minute.ToString(),
                UserId = userId,
                DaysOfWeek = days
            };

            await _repository.AddAsync(notiflexTrigger);
            await _repository.SaveChangesAsync();

            await scheduler.ScheduleJob(trigger);
        }

        public async Task<List<TriggerGetOneDto>> GetAllTriggers(string userId)
        {
            var user = await _userManager.Users.Include(a => a.Triggers).Where(a => a.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException();
            }

            var model = new List<TriggerGetOneDto>();

            foreach (var item in user.Triggers)
            {
                model.Add(
                    new TriggerGetOneDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        City = item.City,
                        Hour = item.Hour,
                        Minutes = item.Minutes,
                        DaySchedule = item.DaysOfWeek
                    });
            }

            return model;
        }

        public async Task DeleteTrigger(int triggerId, string userId)
        {
            var trigger = await _repository.GetByIdAsync<NotiflexTrigger>(triggerId);
            if (trigger == null)
            {
                throw new NotFoundException();
            }
            
            if(trigger.UserId != userId || userId == null)
            {
                throw new ArgumentException("Invalid user!");
            }

            var scheduler = await _schedulerFactory.GetScheduler();
            var key = new TriggerKey(trigger.Identity);

            await _repository.DeleteAsync<NotiflexTrigger>(trigger.Id);
            await scheduler.UnscheduleJob(key);

            await _repository.SaveChangesAsync();
        }

        public async Task<int> GetHourUTC(string cityName, int hour)
        {
            var report = await _modelConfigurer.ConfigureWeatherReport(cityName);

            if (report == null || report?.ToString()?.Length < 1) throw new ArgumentException("Invalid city name");

            int utcOffset = ((report?.TimeZone ?? throw new ArgumentException("Invalid city name")) / 60) / 60;

            int hourUTC;

            if (hour - utcOffset <= 0)
            {
                hourUTC = 24 + (hour - utcOffset);
            }
            else if (hour - utcOffset > 24)
            {
                hourUTC = (hour - utcOffset) - 24;
            }
            else hourUTC = hour - utcOffset;

            if (hourUTC == 24)
            {
                hourUTC = 0;
            }

            return hourUTC;
        }
    }
}
