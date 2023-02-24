using AutoMapper;
using Moq;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Services.AccountServices;
using Notiflex.Core.Services.Contracts;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.Infrastructure.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.UnitTests.Core
{
    public class AccountServiceTests : TestBase
    {
        private IAccountService _accountService;
        private Mock<IMapper> _mapperMock;
        private Mock<IRepository> _repoMock;

        [SetUp]
        public void TestInitialize()
        {
            _mapperMock = new Mock<IMapper>();
            _repoMock= new Mock<IRepository>();
            
            _accountService = new AccountService(UserManager.Object, SignInManager.Object, RoleManager.Object, _repoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task CreateUserAsyncCreatesUser()
        {
            //Arrange
            var user = UsersDataStorage.NotiflexUserApproved;
            var registerDto = new RegisterDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName, 
                Email = user.Email,
                ProfilePic= user.ProfilePic,
                Age= user.Age,
                Gender= user.Gender,
                UserName= user.UserName
            };
            string password = "ValidPass123$";

            //Act
            var result = await _accountService.CreateUserAsync(registerDto, password);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Succeeded, Is.True);
            UserManager.Verify(a => a.CreateAsync(It.IsAny<NotiflexUser>(), It.IsAny<string>()));


        }
    }
}
