using Moq;
using Notiflex.Core.Exceptions;
using Notiflex.Core.Services.BotServices;
using Notiflex.Core.Services.Contracts;
using Notiflex.Core.Services.SchedulerServices;
using Notiflex.Infrastructure.Data.Models.ScheduleModels;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.Infrastructure.Repositories.Contracts;
using Notiflex.UnitTests.Core.Helpers;
using Quartz;

namespace Notiflex.UnitTests.Core
{
    [TestFixture]
    public class TriggerServiceTests : TestBase
    {
        private ITriggerService triggerService;
        private NotiflexTrigger trigger;
        private NotiflexTrigger triggerWrongCity;
        private NotiflexTrigger triggerWrongUserId;
        private NotiflexTrigger triggerWrongTime;
        private NotiflexUser user;
        private NotiflexUser userNoTelegramInfo;
        private NotiflexUser userNoId;
        private List<DayOfWeek> daysSchedule;

        [SetUp]
        public void TestInitialize()
        {
            trigger = TriggersDataStorage.Trigger;
            triggerWrongCity = TriggersDataStorage.TriggerWrongCity;
            triggerWrongUserId = TriggersDataStorage.TriggerWrongUserId;
            triggerWrongTime = TriggersDataStorage.TriggerWrongTime;
            user = UsersDataStorage.NotiflexUserDefault;
            userNoTelegramInfo = UsersDataStorage.NotiflexUserNoTelegramInfo;
            userNoId = UsersDataStorage.NotiflexUserNoId;

            var days = trigger.DaysOfWeek.Split(", ").ToArray();

            daysSchedule = new List<DayOfWeek>();
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
        }

        /// <summary>
        /// Tests for CreateWeatherReportTriggerAsync method.
        /// </summary>
        [Test]
        public async Task CreateWeatherReportTriggerAsyncCreatesReport()
        {
            RepoMock = new Mock<IRepository>();

            triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            var job = JobBuilder
                .Create<ReportSenderJob>()
                .StoreDurably(true)
                .WithIdentity("ReportSenderJob", "fails")
                .Build();

            await Scheduler.Object.AddJob(job, false).ConfigureAwait(false);

            async Task CreateReport() => await triggerService
                .CreateWeatherReportTriggerAsync(user.Id
                    , trigger.Name
                    , trigger.City
                    , user.TelegramInfo
                    , new TimeOfDay(trigger.Hour
                        , int.Parse(trigger.Minutes))
                    , daysSchedule.ToArray()).ConfigureAwait(false);

            Assert.DoesNotThrowAsync(CreateReport);
        }
        [Test]
        public void ThrowArgumentExceptionWhenCityNameInvalid()
        {
            RepoMock = new Mock<IRepository>();
            var modelConfig = ModelConfigurer;

            modelConfig.Setup(x => x.ConvertNameToCoordinates(It.IsAny<string>()))
                .ReturnsAsync(new List<string>()
                {
                    "1",
                    "1"
                });

            triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, modelConfig.Object, UserManager.Object);
            
            Assert.ThrowsAsync<ArgumentException>(() => triggerService
                .CreateWeatherReportTriggerAsync(user.Id
                    , triggerWrongCity.Name
                    , triggerWrongCity.City, user.TelegramInfo
                    , new TimeOfDay(triggerWrongCity.Hour
                        , int.Parse(triggerWrongCity.Minutes))
                    , daysSchedule.ToArray()), "Invalid city name!");

            modelConfig.Setup(x => x.ConvertNameToCoordinates(It.IsAny<string>()))
                .ReturnsAsync(new List<string>()
                {
                    "1",
                    "1",
                    null
                });

            triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, modelConfig.Object, UserManager.Object);

            Assert.ThrowsAsync<ArgumentException>(() => triggerService
                .CreateWeatherReportTriggerAsync(user.Id
                    , triggerWrongCity.Name
                    , triggerWrongCity.City, user.TelegramInfo
                    , new TimeOfDay(triggerWrongCity.Hour
                        , int.Parse(triggerWrongCity.Minutes))
                    , daysSchedule.ToArray()), "Invalid city name!");
        }
        [Test]
        public void ThrowArgumentExceptionWhenUserIdInvalid()
        {
            RepoMock = new Mock<IRepository>();

            triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);
            
            Assert.ThrowsAsync<ArgumentException>(() => triggerService
                .CreateWeatherReportTriggerAsync(userNoId.Id
                    , triggerWrongUserId.Name
                    , triggerWrongUserId.City
                    , user.TelegramInfo
                    , new TimeOfDay(triggerWrongUserId.Hour
                        , int.Parse(triggerWrongUserId.Minutes))
                    , daysSchedule.ToArray()), "Invalid model!");
        }
        [Test]
        public void ThrowArgumentExceptionWhenTelegramDataInvalid()
        {
            RepoMock = new Mock<IRepository>();

            triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            Assert.ThrowsAsync<ArgumentException>(() => triggerService
                .CreateWeatherReportTriggerAsync(userNoTelegramInfo.Id
                    , trigger.Name
                    , trigger.City
                    , userNoTelegramInfo.TelegramInfo
                    , new TimeOfDay(trigger.Hour
                        , int.Parse(trigger.Minutes))
                    , daysSchedule.ToArray()), "Invalid model!");
        }

        /// <summary>
        /// Tests for GetAllTriggers method.
        /// </summary>
        [Test]
        public async Task GetAllTriggersReturnsListOfTriggers()
        {
            RepoMock = new Mock<IRepository>();

            triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            var triggers = await triggerService.GetAllTriggers(user.Id);

            Assert.That(triggers.Any());
        }
        [Test]
        public void ThrowsNotFoundExceptionGetAllTriggersIfUserNotFound()
        {
            RepoMock = new Mock<IRepository>();

            triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            Assert.ThrowsAsync<NotFoundException>(() =>
                triggerService.GetAllTriggers(userNoId.Id));
        }

        /// <summary>
        /// Tests for DeleteTrigger(int triggerId, string userId)
        /// </summary>
        [Test]
        public void DeleteTriggerDeletesGivenTrigger()
        {
            RepoMock = new Mock<IRepository>();

            triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            var beforeDeleteTriggersCount = user.Triggers.Count;

            Assert.DoesNotThrowAsync(() => triggerService.DeleteTrigger(trigger.Id, user.Id));
            Assert.That(user.Triggers, Has.Count.Not.EqualTo(beforeDeleteTriggersCount));
        }
        [Test]
        public void ThrowsNotFoundExceptionWhenTriggerIsNull()
        {
            RepoMock = new Mock<IRepository>();

            triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            Assert.ThrowsAsync<NotFoundException>(() => triggerService.DeleteTrigger(-1, user.Id));
        }
        [Test]
        public void ThrowsArgumentExceptionWhenUserIsNullOrTriggerDoesNotBelongToTHeGivenUser()
        {
            RepoMock = new Mock<IRepository>();

            triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            Assert.ThrowsAsync<ArgumentException>(() => triggerService.DeleteTrigger(trigger.Id, userNoId.Id), "Invalid user!");
            Assert.ThrowsAsync<ArgumentException>(() => triggerService.DeleteTrigger(trigger.Id, "22"), "Invalid user!");
        }

        /// <summary>
        /// Tests for GetHourUtc(string cityName, int hour)
        /// </summary>
        [Test]
        public async Task GetHourUtcConvertsGivenTimeToUtc()
        {
            RepoMock = new Mock<IRepository>();

            triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            var hour = await triggerService.GetHourUtc(trigger.City, 14);
            Assert.That(hour, Is.EqualTo(12));
        }
        [Test]
        public void ThrowsArgumentExceptionWhenCityNameInvalid()
        {
            RepoMock = new Mock<IRepository>();

            triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);
            
            Assert.ThrowsAsync<ArgumentException>(() =>
                triggerService.GetHourUtc(triggerWrongCity.City, 14), "Invalid city name!");
        }
    }
}
