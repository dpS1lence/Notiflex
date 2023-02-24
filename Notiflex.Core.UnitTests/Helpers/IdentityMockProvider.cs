using Microsoft.AspNetCore.Identity;
using Moq;
using Notiflex.Infrastructure.Data.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.UnitTests.Core.Helpers
{
    public class IdentityMockProvider
    {
        private List<NotiflexUser> ls;
        private List<IdentityUserRole<string>> userRoles;
        private List<IdentityRole> roles;
        public IdentityMockProvider(List<NotiflexUser> ls, List<IdentityUserRole<string>> userRoles, List<IdentityRole> roles)
        {
            this.ls = ls;
            this.userRoles = userRoles;
            this.roles = roles; 
        }
        public Mock<UserManager<NotiflexUser>> MockUserManager()
        {

            var store = new Mock<IUserStore<NotiflexUser>>();
            var manager = new Mock<UserManager<NotiflexUser>>(store.Object, null, null, null, null, null, null, null, null);
            manager.Object.UserValidators.Add(new UserValidator<NotiflexUser>());
            manager.Object.PasswordValidators.Add(new PasswordValidator<NotiflexUser>());

            manager.Setup(x => x.DeleteAsync(It.IsAny<NotiflexUser>()))
                .ReturnsAsync(IdentityResult.Success);

            manager.Setup(x => x.CreateAsync(It.IsAny<NotiflexUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<NotiflexUser, string>((x, y) => ls.Add(x));

            manager.Setup(x => x.UpdateAsync(It.IsAny<NotiflexUser>()))
                .ReturnsAsync(IdentityResult.Success);

            manager.Setup(um => um.FindByNameAsync(
                    It.IsAny<string>()))!
                .ReturnsAsync((string username) =>
                    ls.FirstOrDefault(u => u.UserName == username));

            manager.Setup(um => um.FindByIdAsync(
                   It.IsAny<string>()))!
               .ReturnsAsync((string id) =>
                   ls.FirstOrDefault(u => u.Id == id));

            manager.Setup(um => um.FindByEmailAsync(
                   It.IsAny<string>()))!
               .ReturnsAsync((string email) =>
                   ls.FirstOrDefault(u => u.Email == email))
               ;
            manager.Setup(um => um.GenerateEmailConfirmationTokenAsync(
                   It.IsAny<NotiflexUser>()))!
               .ReturnsAsync(Guid.NewGuid().ToString());

            manager.Setup(um => um.GeneratePasswordResetTokenAsync(
                It.IsAny<NotiflexUser>()))!
                .ReturnsAsync(Guid.NewGuid().ToString());

            manager.Setup(um => um.ResetPasswordAsync(It.IsAny<NotiflexUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            manager.Setup(um => um.ConfirmEmailAsync(It.IsAny<NotiflexUser>(), It.IsAny<string>()))!
                .ReturnsAsync(IdentityResult.Success);


            manager.Setup(um => um.Users)
                .Returns(ls != null ? ls.AsQueryable() : null);

            manager.Setup(um => um.IsInRoleAsync(It.IsAny<NotiflexUser>(), It.IsAny<string>()))!
                .ReturnsAsync((NotiflexUser user, string role) =>
                {
                    if (userRoles == null || roles == null)
                    {
                        return false;
                    }
                    var ids = userRoles.Where(a => a.UserId == user.Id).Select(r => r.RoleId).ToList();
                    var roleList = roles.Where(r => ids.Contains(r.Id));
                    return roleList.Any(rl => rl.Name == role);
                }
                );
               

            return manager;

        }
        public Mock<SignInManager<NotiflexUser>> MockSignInManager()
        {
            
            var manager = new Mock<SignInManager<NotiflexUser>>(MockUserManager().Object, null, null ,null, null ,null);

            manager.Setup(sm => sm.PasswordSignInAsync(It.IsAny<NotiflexUser>(), It.IsAny<string>(), false, false))!
                .ReturnsAsync(SignInResult.Success);
            return manager;
        }

        public Mock<RoleManager<IdentityRole>> MockRoleManager()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            var manager = new Mock<RoleManager<IdentityRole>>(store.Object, null, null, null, null);

            manager.Object.RoleValidators.Add(new RoleValidator<IdentityRole>());

            manager.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))!
                .ReturnsAsync((string rolename) => roles.Any(a => a.Name == rolename));
            return manager;
        }
    }
}
