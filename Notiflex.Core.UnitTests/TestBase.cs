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
        protected Mock<UserManager<NotiflexUser>> _userManager = null!;
        protected Mock<SignInManager<NotiflexUser>> _signInManager = null!;
        protected Mock<RoleManager<IdentityRole>> _roleManager = null!;
        protected Mock<IModelConfigurer> _modelConfigurer = null!;
        protected Mock<ISchedulerFactory> _schedulerFactory = null!;
        protected Mock<IScheduler> _scheduler = null!;
        protected UsersDataStorage _usersDataStorage = null!;
        protected TriggersDataStorage _triggersDataStorage = null!;
        protected Mock<IRepository>? repoMock;

        [SetUp]
        public void Setup()
        {
            _usersDataStorage = new UsersDataStorage();
            _triggersDataStorage = new TriggersDataStorage();

            _userManager = IdentityMockProvider.MockUserManager(_usersDataStorage.Users, _usersDataStorage.UserRoles, _usersDataStorage.Roles);
            _signInManager = IdentityMockProvider.MockSignInManager();
            _roleManager = IdentityMockProvider.MockRoleManager(_usersDataStorage.Roles);
            _modelConfigurer = ModelConfigurerMockProvider.MockModelConfigurer();
            _schedulerFactory = SchedulerFactoryMockProvider.MockSchedulerFactory();
            _scheduler = SchedulerMockProvider.MockScheduler();
        }
    }
}
