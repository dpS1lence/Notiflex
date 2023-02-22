using Microsoft.AspNetCore.Identity;
using Moq;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.UnitTests.Common.DataStorages;
using Notiflex.UnitTests.Core.Helpers;
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
        protected UsersDataStorage _usersDataStorage = null!;
        
        [SetUp]
        public void Setup()
        {
            _usersDataStorage = new UsersDataStorage();

            _userManager = MockProvider.MockUserManager(_usersDataStorage.Users, _usersDataStorage.UserRoles, _usersDataStorage.Roles);
            _signInManager = MockProvider.MockSignInManager();
            _roleManager = MockProvider.MockRoleManager(_usersDataStorage.Roles);
        }
    }
}
