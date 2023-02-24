using AutoMapper;
using MockQueryable.Moq;
using Moq;
using Notiflex.Core.Exceptions;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Services.AccountServices;
using Notiflex.Core.Services.Contracts;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.Infrastructure.Repositories.Contracts;
using Org.BouncyCastle.Asn1.Esf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            _repoMock.Setup(a => a.AllReadonly<NotiflexUser>(It.IsAny<Expression<Func<NotiflexUser, bool>>>()))
                .Returns((Expression<Func<NotiflexUser, bool>> exp) => UsersDataStorage.Users.Where(exp.Compile()).AsQueryable().BuildMock());

            _repoMock.Setup(a => a.All<NotiflexUser>(It.IsAny<Expression<Func<NotiflexUser, bool>>>()))
                .Returns((Expression<Func<NotiflexUser, bool>> exp) => UsersDataStorage.Users.Where(exp.Compile()).AsQueryable().BuildMock());

            _repoMock.Setup(a => a.GetByIdAsync<NotiflexUser>(It.IsAny<string>()))!.ReturnsAsync((string id) => UsersDataStorage.Users.FirstOrDefault(a => a.Id == id));

            _mapperMock.Setup(a => a.Map<ProfileDto>(It.IsAny<NotiflexUser>()))!.Returns((NotiflexUser user) =>
            {
                var profileDto = new ProfileDto()
                {
                    DefaultTime = user.DefaultTime,
                    Description = user.Description,
                    FirstName = user.FirstName,
                    HomeTown = user.HomeTown,
                    LastName = user.LastName,
                    ProfilePic = user.ProfilePic,
                    TelegramChatId = user.TelegramInfo
                };
                return profileDto;
            });


            _accountService = new AccountService(UserManager.Object, SignInManager.Object, RoleManager.Object, _repoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task CreateUserAsyncCreatesUser()
        {
            //Arrange
            
            var registerDto = new RegisterDto()
            {
                FirstName = "Pesho",
                LastName = "Pesho", 
                Email = "emailthatdoesntexist@email.email",
                ProfilePic= "pfp.pfp",
                Age= "20",
                Gender= "Male",
                UserName= "Peshkata"
            };
            string password = "ValidPass123$";

            //Act
            var result = await _accountService.CreateUserAsync(registerDto, password);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Succeeded, Is.True);
            UserManager.Verify(a => a.CreateAsync(It.IsAny<NotiflexUser>(), It.IsAny<string>()));


        }
        [Test]
        public async Task CreateUserAsyncThrowsWhenUserIsInvalid()
        {
            //Arrange
            var registerDto = new RegisterDto()
            {
                FirstName = "",
                LastName = "Pesho",
                Email = "emailthatdoesntexist@email.email",
                ProfilePic = "pfp.pfp",
                Age = "20",
                Gender = "Male",
                UserName = "Peshkata"
            };
            string password = "ValidPass123$";

            //Assert
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _accountService.CreateUserAsync(registerDto, password);
            });
        }
        [Test]
        public async Task CreateUserAsyncReturnsFailedWhenUserExists()
        {
            //Arrange
            var user = UsersDataStorage.NotiflexUserApproved;
            var registerDto = new RegisterDto()
            {
                Age = user.Age,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                Gender = user.Gender,
                LastName = user.LastName,
                ProfilePic = user.ProfilePic,
            };
            string password = "ValidPass123$";

            //Act
            var result = await _accountService.CreateUserAsync(registerDto, password);
            //Assert
            Assert.That(result.Succeeded, Is.EqualTo(false));
        }
        [Test]
        public async Task IsEmailConfirmedAsyncReturnsTrueWithConfirmedEmail()
        {
            //Arrange
            var user = UsersDataStorage.NotiflexUserDefault;
            string email = user.Email;

            //Act
            var result = await _accountService.IsEmailConfirmedAsync(email);

            //Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public async Task IsEmailConfirmedAsyncThrowsWhenEmailDoesntExist()
        {
            //Arrange            
            string email = "randomemailThatDoesntExist@email.email";

            //Assert
            Assert.ThrowsAsync<NotFoundException>(() =>  _accountService.IsEmailConfirmedAsync(email));

        }
        [Test]
        public async Task SignInUserAsyncReturnsSuccess()
        {
            //Arrange
            var user = UsersDataStorage.NotiflexUserDefault;
            string email = user.Email;
            string password = "Password123$";

            //Act
            var result = await _accountService.SignInUserAsync(email, password);

            //Assert
            Assert.That(result.Succeeded, Is.EqualTo(true));
            SignInManager.Verify(x => x.PasswordSignInAsync(user, password, false ,false));
        }
        [Test]
        public async Task SignInUserAsyncThrowsWhenUserWithEmailDoesntExist()
        {
            //Arrange

            string email = "randomemailThatDoesntExist@email.email";
            string password = "Password123$";

            //Act
            var result = await _accountService.SignInUserAsync(email, password);

            //Assert
            Assert.That(result.Succeeded, Is.EqualTo(false));
        }
        [Test]
        public async Task UserExistsByEmailReturnsTrueWhenUserExists()
        {
            //Arrange
            var user = UsersDataStorage.NotiflexUserDefault;
            string email = user.Email;
            

            //Act
            var result = await _accountService.UserExistsByEmail(email);

            //Assert
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public async Task GetUserIdByEmailReturnsCorrectID()
        {
            //Arrange
            var user = UsersDataStorage.NotiflexUserDefault;
            string email = user.Email;
            string id = user.Id;


            //Act
            var result = await _accountService.GetUserIdByEmail(email);

            //Assert
            Assert.That(result, Is.EqualTo(id));
        }
        [Test]
        public async Task SignOutAsyncWorks()
        {
            //Act
            await _accountService.SignOutAsync();
            //Assert
            SignInManager.Verify(a => a.SignOutAsync());
        }
        [Test]
        public async Task GenerateEmailConfirmationTokenReturnsToken()
        {
            //Arrange
            var user = UsersDataStorage.NotiflexUserDefault;
            string id = user.Id;


            //Act
            var result = await _accountService.GenerateEmailConfirmationTokenAsync(id);

            //Assert
            Assert.That(string.IsNullOrWhiteSpace(result), Is.False);
            Assert.That(result, Is.AssignableFrom<string>());
            _repoMock.Verify(a => a.GetByIdAsync<NotiflexUser>(id));
        }
        [Test]
        public async Task GenerateEmailConfirmationTokenThrowsWhenUserIdIsInvalid()
        {
            //Arrange
            string id = "-1";

            //Assert
            Assert.ThrowsAsync<NotFoundException>(() =>
            {
                return _accountService.GenerateEmailConfirmationTokenAsync(id);
            });
        }
        [Test]
        public async Task GeneratePasswordResetTokenReturnsToken()
        {
            //Arrange
            var user = UsersDataStorage.NotiflexUserDefault;
            string id = user.Id;


            //Act
            var result = await _accountService.GeneratePasswordResetTokenAsync(id);

            //Assert
            Assert.That(string.IsNullOrWhiteSpace(result), Is.False);
            Assert.That(result, Is.AssignableFrom<string>());
            _repoMock.Verify(a => a.GetByIdAsync<NotiflexUser>(id));
        }
        [Test]
        public async Task GeneratePasswordThrowsWhenUserIdIsInvalid()
        {
            //Arrange
            string id = "-1";

            //Assert
            Assert.ThrowsAsync<NotFoundException>(() =>
            {
                return _accountService.GeneratePasswordResetTokenAsync(id);
            });
        }
        [Test]
        public async Task ResetPasswordAsyncWorks()
        {
            //Arrange
            var user = UsersDataStorage.NotiflexUserDefault;
            string email = user.Email;
            string id = user.Id;
            string token = Guid.NewGuid().ToString();
            string newpass = "NewPass123$";


            //Act
            var result = await _accountService.ResetPasswordAsync(email, token, newpass);

            //Assert
            Assert.That(result.Succeeded, Is.EqualTo(true));

            SignInManager.Verify(a => a.RefreshSignInAsync(It.IsAny<NotiflexUser>()));
            UserManager.Verify(a => a.ResetPasswordAsync(It.IsAny<NotiflexUser>(), token, newpass));
        }
        [Test]
        public async Task ResetPasswordAsyncThrowsWhenEmailIsInvalid()
        {
            //Arrange
            var user = UsersDataStorage.NotiflexUserDefault;
            string email = "invalid";
            string token = Guid.NewGuid().ToString();
            string newpass = "NewPass123$";

            //Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _accountService.ResetPasswordAsync(email, token, newpass));
        }
        [Test]
        public async Task ResetPasswordAsyncThrowsWhenArgumentsInvalid()
        {
            //Arrange
            var user = UsersDataStorage.NotiflexUserDefault;
            string email = user.Email;
            string token = "";
            string newpass = "";

            //Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _accountService.ResetPasswordAsync(email, token, newpass));
        }
        [Test]
        public async Task ConfirmEmailAsyncWorks()
        {
            //Arrange
            var user = UsersDataStorage.NotiflexUserDefault;

            string id = user.Id;
            string token = Guid.NewGuid().ToString();


            //Act
            var result = await _accountService.ConfirmEmailAsync(id, token);

            //Assert
            Assert.That(result.Succeeded, Is.EqualTo(true));
        }
        [Test]
        public async Task ResetPasswordAsyncThrowsWhenIdIsInvalid()
        {
            //Arrange
            var user = UsersDataStorage.NotiflexUserDefault;
            string id = "-1";
            string token = Guid.NewGuid().ToString();

            //Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _accountService.ConfirmEmailAsync(id, token));
        }
        [Test]
        public async Task EditProfileWorks()
        {
            var user = UsersDataStorage.NotiflexUserDefault;
            var userDto = new ProfileDto()
            {
                DefaultTime = user.DefaultTime,
                Description = user.Description,
                FirstName = user.FirstName,
                HomeTown = user.HomeTown,
                LastName = user.LastName,
                ProfilePic = user.ProfilePic,
                TelegramChatId = user.TelegramInfo
            };

            await _accountService.EditProfile(user.Id, userDto);

            _repoMock.Verify(a => a.Update(It.IsAny<NotiflexUser>()));
            _repoMock.Verify(a => a.SaveChangesAsync());

        }
        [Test]
        public async Task EditProfileThrowsWithInvalidDto()
        {
            var user = UsersDataStorage.NotiflexUserDefault;

            var userDto = new ProfileDto()
            {
                DefaultTime = user.DefaultTime,
                Description = user.Description,
                FirstName =  "",
                HomeTown = user.HomeTown,
                LastName = "",
                ProfilePic = user.ProfilePic,
                TelegramChatId = user.TelegramInfo
            };

            Assert.ThrowsAsync<ArgumentException>(() =>  _accountService.EditProfile(user.Id, userDto));


        }
        [Test]
        public async Task EditProfileThrowsWithInvalidId()
        {
            var user = UsersDataStorage.NotiflexUserDefault;

            var userDto = new ProfileDto()
            {
                DefaultTime = user.DefaultTime,
                Description = user.Description,
                FirstName = user.FirstName,
                HomeTown = user.HomeTown,
                LastName = user.LastName,
                ProfilePic = user.ProfilePic,
                TelegramChatId = user.TelegramInfo
            };

            Assert.ThrowsAsync<NotFoundException>(() => _accountService.EditProfile("-1", userDto));
        }

        [Test]
        public async Task GetUserDataWorks()
        {
            var user = UsersDataStorage.NotiflexUserDefault;
            var id = user.Id;

            var result = await _accountService.GetUserData(id);

            var profileDto = new ProfileDto()
            {
                DefaultTime = user.DefaultTime,
                Description = user.Description,
                FirstName = user.FirstName,
                HomeTown = user.HomeTown,
                LastName = user.LastName,
                ProfilePic = user.ProfilePic,
                TelegramChatId = user.TelegramInfo
            };

            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<ProfileDto>(profileDto);   

        }
        [Test]
        public async Task GetUserDataThrowsWhenIdIsInvalid()
        {
            var user = UsersDataStorage.NotiflexUserDefault;
            var id = "-1";

            Assert.ThrowsAsync<NotFoundException>(() =>  _accountService.GetUserData(id));
        }

        [Test]
        public async Task AproveUserWorks()
        {
            var user = UsersDataStorage.NotiflexUserDefault;
            var id = user.Id;

            await _accountService.AproveUser(id, "01", "Tarnovo", "photo.photo");

            _repoMock.Verify(a => a.Update(It.IsAny<NotiflexUser>()));
            _repoMock.Verify(a => a.SaveChangesAsync());
            UserManager.Verify(a => a.AddToRoleAsync(It.IsAny<NotiflexUser>(), "ApprovedUser"));
        }
        
    }
}
