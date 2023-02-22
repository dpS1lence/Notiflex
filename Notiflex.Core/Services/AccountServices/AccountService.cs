using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Notiflex.Core.Exceptions;
using Notiflex.Core.Helpers;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Services.Contracts;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.Infrastructure.Repositories.Contracts;
using Quartz;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<NotiflexUser> _userManager;
        private readonly SignInManager<NotiflexUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository _repo;
        private readonly IMapper _mapper;

        public AccountService(
               UserManager<NotiflexUser> userManager,
               SignInManager<NotiflexUser> signInManager,
               RoleManager<IdentityRole> roleManager,
               IRepository repo,
               IMapper mapper
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repo = repo;
            _mapper = mapper;
            _roleManager = roleManager;
        }
        /// <inheritdoc />
        public async Task<IdentityResult> CreateUserAsync(RegisterDto userDto, string password)
        {
            if (!userDto.IsValid())
            {
                throw new ArgumentException("Invalid userDto.");
            }
            var user = new NotiflexUser
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.UserName,
                Age = userDto.Age,
                Gender = userDto.Gender,
                ProfilePic = userDto.ProfilePic,
            };
            if (await _userManager.FindByEmailAsync(userDto.Email) != null)
            {
                return IdentityResult.Failed();
            }

            IdentityResult result = await _userManager.CreateAsync(user, password);
            return result;
        }
        /// <inheritdoc />
        public async Task<bool> IsEmailConfirmedAsync(string email)
        {
            NotiflexUser? user = await _repo.AllReadonly<NotiflexUser>(u => u.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException();
            }
            return user.EmailConfirmed;
        }
        /// <inheritdoc />
        public async Task<SignInResult> SignInUserAsync(string email, string password)
        {
            NotiflexUser? user = await _repo.AllReadonly<NotiflexUser>(u => u.Email == email)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return SignInResult.Failed;
            }            

            return await _signInManager.PasswordSignInAsync(
                user,
                password,
                false,
                false);
        }
        /// <inheritdoc />
        public async Task<bool> UserExistsByEmail(string email)
        {
            return await this._repo.AllReadonly<NotiflexUser>(u => u.Email == email)
                .FirstOrDefaultAsync() != null;
        }
        /// <inheritdoc />
        public async Task<string> GetUserIdByEmail(string email)
        {
            NotiflexUser? user = await this._repo.AllReadonly<NotiflexUser>(
                    u => u.Email == email)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException();
            }

            return user.Id;
        }
        /// <inheritdoc />
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        /// <inheritdoc />
        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            NotiflexUser user = await _repo.GetByIdAsync<NotiflexUser>(userId);
            if (user == null)
            {
                throw new NotFoundException();
            }
            await _userManager.UpdateSecurityStampAsync(user);
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        }
        /// <inheritdoc />
        public async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            
            NotiflexUser user = await _repo.GetByIdAsync<NotiflexUser>(userId);
            if (user == null)
            {
                throw new NotFoundException();
            }
            await _userManager.UpdateSecurityStampAsync(user);

            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        }
        /// <inheritdoc />
        public async Task<IdentityResult> ResetPasswordAsync(string email, string code, string newPassword)
        {
            NotiflexUser? user = await _repo.All<NotiflexUser>(u => u.Email == email)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException();
            }
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException();
            }
            IdentityResult result = await _userManager.ResetPasswordAsync(user, code, newPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
            }
            return result;
        }
        /// <inheritdoc />
        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            NotiflexUser user = await _repo.GetByIdAsync<NotiflexUser>(userId);
            if (user == null)
            {
                throw new NotFoundException();
            }
            string token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            return await _userManager.ConfirmEmailAsync(user, token);

        }
        /// <inheritdoc />
        public async Task EditProfile(string userId, ProfileDto model)
        {
            if (!model.IsValid())
            {
                throw new ArgumentException();
            }
            NotiflexUser user = await _repo.GetByIdAsync<NotiflexUser>(userId);

            if (user == null)
            {
                throw new NotFoundException();
            }

            user.TelegramInfo = model.TelegramChatId;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Description = model.Description;
            user.ProfilePic = model.ProfilePic;
            user.DefaultTime = model.DefaultTime;
            user.HomeTown = model.HomeTown;

            _repo.Update(user);

            await _repo.SaveChangesAsync();
        }
        /// <inheritdoc />

        public async Task<ProfileDto> GetUserData(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException();

            }
            var profileDto = _mapper.Map<ProfileDto>(user);

            return profileDto;
        }

        /// <inheritdoc />
        public async Task AprooveUser(string userId, string telegramId, string hometown, string photo)
        {
            string roleName = "ApprovedUser";
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                IdentityRole role = new IdentityRole(roleName);
                await _roleManager.CreateAsync(role);
            }
            NotiflexUser user = await _repo.GetByIdAsync<NotiflexUser>(userId);
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                return;
            }
            if (string.IsNullOrEmpty(telegramId) || string.IsNullOrEmpty(hometown) || string.IsNullOrEmpty(photo))
            {
                throw new ArgumentException();
            }
            if (user != null)
            {
                
                user.TelegramInfo = telegramId;
                user.HomeTown = hometown;
                user.ProfilePic = photo;

                _repo.Update(user);
                await _repo.SaveChangesAsync();
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }
        /// <inheritdoc />
        public async Task<bool> IsInRole(string userId, string roleName)
        {
            NotiflexUser user = await _repo.GetByIdAsync<NotiflexUser>(userId);
            return await _userManager.IsInRoleAsync(user, roleName);
        }
    }
}
