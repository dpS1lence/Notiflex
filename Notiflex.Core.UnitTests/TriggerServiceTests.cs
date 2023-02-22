using Moq;
using Notiflex.Core.Services.Contracts;
using Notiflex.Core.Services.SchedulerServices;
using Notiflex.Infrastructure.Repositories.Contracts;
using Notiflex.UnitTests.Core.Helpers;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.UnitTests.Core
{
    public class TriggerServiceTests : TestBase
    {
        [SetUp]
        public void TestInitialize()
        {

        }

        [Test]
        public async Task CreateWeatherReportTriggerAsyncCreatesReport()
        {
            repoMock = new Mock<IRepository>();

            ITriggerService triggerService = new TriggerService(_schedulerFactory.Object, repoMock.Object, _modelConfigurer.Object, _userManager.Object);

            var trigger = _triggersDataStorage.Trigger;

            var user = _usersDataStorage.NotiflexUserDefault;

            string[] days = trigger.DaysOfWeek.Split(", ").ToArray();

            List<DayOfWeek> daysSchedule = new();
            foreach (var item in days)
            {
                switch (item.ToLower())
                {
                    case "monday":
                        daysSchedule.Add(DayOfWeek.Monday); break;
                    case "tuesday":
                        daysSchedule.Add(DayOfWeek.Tuesday); break;
                    case "wednesday":
                        daysSchedule.Add(DayOfWeek.Wednesday); break;
                    case "thursday":
                        daysSchedule.Add(DayOfWeek.Thursday); break;
                    case "friday":
                        daysSchedule.Add(DayOfWeek.Friday); break;
                    case "saturday":
                        daysSchedule.Add(DayOfWeek.Saturday); break;
                    case "sunday":
                        daysSchedule.Add(DayOfWeek.Sunday); break;
                }
            }

            IJobDetail job = JobBuilder
                .Create<ReportSenderJob>()
                .StoreDurably(true)
                .WithIdentity("ReportSenderJob", "fails")
                .Build();

            await _scheduler.Object.AddJob(job, false);

            Assert.DoesNotThrowAsync(async () => await triggerService.CreateWeatherReportTriggerAsync(user.Id, trigger.Name, trigger.City, user.TelegramInfo, new TimeOfDay(trigger.Hour, int.Parse(trigger.Minutes)), daysSchedule.ToArray()));
        }
    }
}
