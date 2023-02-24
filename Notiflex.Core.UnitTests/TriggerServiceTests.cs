using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
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
        private ITriggerService _triggerService = null!;
        private NotiflexTrigger _trigger = null!;
        private NotiflexTrigger _triggerWrongCity = null!;
        private NotiflexTrigger _triggerWrongUserId = null!;
        private NotiflexUser _user = null!;
        private NotiflexUser _userNoTelegramInfo = null!;
        private NotiflexUser _userNoId = null!;
        private List<DayOfWeek> _daysSchedule = null!;

        [SetUp]
        public void TestInitialize()
        {
            _trigger = TriggersDataStorage.Trigger;
            _triggerWrongCity = TriggersDataStorage.TriggerWrongCity;
            _triggerWrongUserId = TriggersDataStorage.TriggerWrongUserId;
            _user = UsersDataStorage.NotiflexUserDefault;
            _userNoTelegramInfo = UsersDataStorage.NotiflexUserNoTelegramInfo;
            _userNoId = UsersDataStorage.NotiflexUserNoId;

            var days = _trigger.DaysOfWeek.Split(", ").ToArray();

            _daysSchedule = new List<DayOfWeek>();
            foreach (var item in days)
            {
                switch (item.ToLower())
                {
                    case "monday":
                        _daysSchedule.Add(DayOfWeek.Monday); break;
                    case "tuesday":
                        _daysSchedule.Add(DayOfWeek.Tuesday); break;
                    case "wednesday":
                        _daysSchedule.Add(DayOfWeek.Wednesday); break;
                    case "thursday":
                        _daysSchedule.Add(DayOfWeek.Thursday); break;
                    case "friday":
                        _daysSchedule.Add(DayOfWeek.Friday); break;
                    case "saturday":
                        _daysSchedule.Add(DayOfWeek.Saturday); break;
                    case "sunday":
                        _daysSchedule.Add(DayOfWeek.Sunday); break;
                }
            }
        }

        /// <summary>
        /// Tests for CreateWeatherReportTriggerAsync method.
        /// </summary>
        [Test]
        public async Task CreateWeatherReportTriggerAsyncCreatesReport()
        {
            // Set up mock repository
            RepoMock = new Mock<IRepository>();

            // Create trigger service
            _triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            // Call the method being tested and create the job
            var job = JobBuilder
                .Create<ReportSenderJob>()
                .StoreDurably(true)
                .WithIdentity("ReportSenderJob", "fails")
                .Build();

            await Scheduler.Object.AddJob(job, false).ConfigureAwait(false);

            // Assert that the result doesn't throw exception
            async Task CreateReport()
            {
                if (_user.TelegramInfo != null)
                    await _triggerService
                        .CreateWeatherReportTriggerAsync(_user.Id
                            , _trigger.Name
                            , _trigger.City
                            , _user.TelegramInfo
                            , new TimeOfDay(_trigger.Hour
                                , int.Parse(_trigger.Minutes))
                            , _daysSchedule.ToArray())
                        .ConfigureAwait(false);
            }

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

            _triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, modelConfig.Object, UserManager.Object);
            
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                Debug.Assert(_user.TelegramInfo != null, "_user.TelegramInfo != null");
                return _triggerService
                    .CreateWeatherReportTriggerAsync(_user.Id
                        , _triggerWrongCity.Name
                        , _triggerWrongCity.City, _user.TelegramInfo
                        , new TimeOfDay(_triggerWrongCity.Hour
                            , int.Parse(_triggerWrongCity.Minutes))
                        , _daysSchedule.ToArray());
            }, "Invalid city name!");

            modelConfig.Setup(x => x.ConvertNameToCoordinates(It.IsAny<string>()))
                .ReturnsAsync(new List<string?>()
                {
                    "1",
                    "1",
                    null
                }!);

            _triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, modelConfig.Object, UserManager.Object);

            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                Debug.Assert(_user.TelegramInfo != null, "_user.TelegramInfo != null");
                return _triggerService
                    .CreateWeatherReportTriggerAsync(_user.Id
                        , _triggerWrongCity.Name
                        , _triggerWrongCity.City, _user.TelegramInfo
                        , new TimeOfDay(_triggerWrongCity.Hour
                            , int.Parse(_triggerWrongCity.Minutes))
                        , _daysSchedule.ToArray());
            }, "Invalid city name!");
        }
        [Test]
        public void ThrowArgumentExceptionWhenUserIdInvalid()
        {
            RepoMock = new Mock<IRepository>();

            _triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);
            
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                Debug.Assert(_user.TelegramInfo != null, "_user.TelegramInfo != null");
                return _triggerService
                    .CreateWeatherReportTriggerAsync(_userNoId.Id
                        , _triggerWrongUserId.Name
                        , _triggerWrongUserId.City
                        , _user.TelegramInfo
                        , new TimeOfDay(_triggerWrongUserId.Hour
                            , int.Parse(_triggerWrongUserId.Minutes))
                        , _daysSchedule.ToArray());
            }, "Invalid model!");
        }
        [Test]
        public void ThrowArgumentExceptionWhenTelegramDataInvalid()
        {
            RepoMock = new Mock<IRepository>();

            _triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            Assert.ThrowsAsync<ArgumentException>(() => _triggerService
                .CreateWeatherReportTriggerAsync(
                    _userNoTelegramInfo.Id
                    , _trigger.Name
                    , _trigger.City
                    , _userNoTelegramInfo.TelegramInfo!
                    , new TimeOfDay(_trigger.Hour
                        , int.Parse(_trigger.Minutes))
                    , _daysSchedule.ToArray()), "Invalid model!");
        }

        /// <summary>
        /// Tests for GetAllTriggers method.
        /// </summary>
        [Test]
        public async Task GetAllTriggersReturnsListOfTriggers()
        {
            // Set up mock repository
            RepoMock = new Mock<IRepository>();

            UserManager.Setup(r => r.Users)!.Returns(UsersDataStorage
                .Users.BuildMock());

            // Create trigger service
            _triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            var triggers = await _triggerService.GetAllTriggers(_user.Id);

            // Assert that the sequence contains any elements
            Assert.That(triggers.Any());
            UserManager.Verify(r => r.Users);
        }
        [Test]
        public void ThrowsNotFoundExceptionGetAllTriggersIfUserNotFound()
        {
            RepoMock = new Mock<IRepository>();

            UserManager.Setup(r => r.Users)!.Returns(UsersDataStorage
                .Users.BuildMock());

            _triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            Assert.ThrowsAsync<NotFoundException>(() =>
                _triggerService.GetAllTriggers(_userNoId.Id));
            UserManager.Verify(r => r.Users);
        }

        /// <summary>
        /// Tests for DeleteTrigger(int triggerId, string userId)
        /// </summary>
        [Test]
        public void DeleteTriggerDeletesGivenTrigger()
        {
            // Set up mock repository
            RepoMock = new Mock<IRepository>();

            RepoMock.Setup(a => 
                a.GetByIdAsync<NotiflexTrigger>(It.IsAny<int>()))!
                .ReturnsAsync((int id) => TriggersDataStorage.Triggers.FirstOrDefault(a => a.Id == id));
            RepoMock.Setup(a => 
                a.DeleteAsync<NotiflexTrigger>(It.IsAny<int>()));

            // Create trigger service
            _triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            // Assert that the result doesn't throw exception
            Assert.DoesNotThrowAsync(() => _triggerService.DeleteTrigger(_trigger.Id, _user.Id));
            RepoMock.Verify(a => a.GetByIdAsync<NotiflexTrigger>(It.IsAny<int>()));
            RepoMock.Verify(a => a.DeleteAsync<NotiflexTrigger>(It.IsAny<int>()));
        }
        [Test]
        public void ThrowsNotFoundExceptionWhenTriggerIsNull()
        {
            RepoMock = new Mock<IRepository>();

            _triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            Assert.ThrowsAsync<NotFoundException>(() => _triggerService.DeleteTrigger(-1, _user.Id));
        }
        [Test]
        public void ThrowsArgumentExceptionWhenUserIsNullOrTriggerDoesNotBelongToTHeGivenUser()
        {
            RepoMock = new Mock<IRepository>();
            RepoMock.Setup(a =>
                    a.GetByIdAsync<NotiflexTrigger>(It.IsAny<int>()))!
                .ReturnsAsync((int id) => TriggersDataStorage.Triggers.FirstOrDefault(a => a.Id == id));

            _triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            Assert.ThrowsAsync<ArgumentException>(() => _triggerService.DeleteTrigger(_trigger.Id, _userNoId.Id), "Invalid user!");
            Assert.ThrowsAsync<ArgumentException>(() => _triggerService.DeleteTrigger(_trigger.Id, "22"), "Invalid user!");
            RepoMock.Verify(a => a.GetByIdAsync<NotiflexTrigger>(It.IsAny<int>()));
        }

        /// <summary>
        /// Tests for GetHourUtc(string cityName, int hour)
        /// </summary>
        [Test]
        public async Task GetHourUtcConvertsGivenTimeToUtc()
        {
            // Set up mock repository
            RepoMock = new Mock<IRepository>();

            // Create trigger service
            _triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);

            var hour = await _triggerService.GetHourUtc(_trigger.City, 14);

            // Assert that the result is equal to the expected
            Assert.That(hour, Is.EqualTo(11));
        }
        [Test]
        public void ThrowsArgumentExceptionWhenCityNameInvalid()
        {
            RepoMock = new Mock<IRepository>();

            _triggerService = new TriggerService(SchedulerFactory.Object, RepoMock.Object, ModelConfigurer.Object, UserManager.Object);
            
            Assert.ThrowsAsync<ArgumentException>(() =>
                _triggerService.GetHourUtc(_triggerWrongCity.City, 14), "Invalid city name!");
        }
    }
}