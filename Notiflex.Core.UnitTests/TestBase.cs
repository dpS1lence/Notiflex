using Microsoft.AspNetCore.Identity;
using Moq;
using Notiflex.Core.Services.Contracts;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.Infrastructure.Repositories.Contracts;
using Notiflex.UnitTests.Common.DataStorages;
using Notiflex.UnitTests.Core.Helpers;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.UnitTests.Core
{
    public abstract class TestBase
    {
        protected Mock<UserManager<NotiflexUser>> UserManager = null!;
        protected Mock<SignInManager<NotiflexUser>> SignInManager = null!;
        protected Mock<RoleManager<IdentityRole>> RoleManager = null!;
        protected Mock<IModelConfigurer> ModelConfigurer = null!;
        protected Mock<ISchedulerFactory> SchedulerFactory = null!;
        protected Mock<IScheduler> Scheduler = null!;
        protected UsersDataStorage UsersDataStorage = null!;
        protected TriggersDataStorage TriggersDataStorage = null!;
        protected Mock<IRepository>? RepoMock;

        [SetUp]
        public void Setup()
        {
            UsersDataStorage = new UsersDataStorage();
            TriggersDataStorage = new TriggersDataStorage();

            UserManager = IdentityMockProvider.MockUserManager(UsersDataStorage.Users, UsersDataStorage.UserRoles, UsersDataStorage.Roles);
            SignInManager = IdentityMockProvider.MockSignInManager();
            RoleManager = IdentityMockProvider.MockRoleManager(UsersDataStorage.Roles);
            ModelConfigurer = ModelConfigurerMockProvider.MockModelConfigurer();
            SchedulerFactory = SchedulerFactoryMockProvider.MockSchedulerFactory();
            Scheduler = SchedulerMockProvider.MockScheduler();
        }
    }
}
