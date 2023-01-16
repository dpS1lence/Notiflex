using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Notiflex.Core.Exceptions;
using Notiflex.Core.Services.Contracts;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.Infrastructure.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<NotiflexUser> _userManager;
        private readonly SignInManager<NotiflexUser> _signInManager;
        private readonly IRepository _repo;
        public AccountService(
           UserManager<NotiflexUser> userManager,
           SignInManager<NotiflexUser> signInManager,
           IRepository repo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repo = repo;
        }
        public async Task<IdentityResult> CreateUserAsync(
           string email,
           string firstName,
           string lastName,
           string userName,
           string password)
        {
            NotiflexUser user = new NotiflexUser
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
            };

            IdentityResult result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }
        public async Task<bool> IsEmailConfirmedAsync(string email)
        {
            NotiflexUser? user = await _repo.AllReadonly<NotiflexUser>(u => u.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException();
            }
            return user.EmailConfirmed;
        }
        public async Task<SignInResult> SignInUserAsync(string email, string password)
        {
            NotiflexUser? user = await _repo.AllReadonly<NotiflexUser>(u => u.Email == email)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException();
            }

            return await _signInManager.PasswordSignInAsync(
                user,
                password,
                false,
                false);
        }
        public async Task<bool> UserExistsByEmail(string email)
        {
            return await this._repo.AllReadonly<NotiflexUser>(u => u.Email == email)
                .FirstOrDefaultAsync() != null;
        }
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
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            NotiflexUser user = await _repo.GetByIdAsync<NotiflexUser>(userId);
            if (user == null)
            {
                throw new NotFoundException();
            }

            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        }
        public async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            NotiflexUser user = await _repo.GetByIdAsync<NotiflexUser>(userId);
            if (user == null)
            {
                throw new NotFoundException();
            }

            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        }
        public async Task<IdentityResult> ResetPasswordAsync(string email, string code, string newPassword)
        {
            NotiflexUser? user = await _repo.AllReadonly<NotiflexUser>(u => u.Email == email)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException();
            }
            IdentityResult result = await _userManager.ChangePasswordAsync(user, code, newPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
            }
            return result;
        }
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
    }
}
