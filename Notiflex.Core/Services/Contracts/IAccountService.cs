using Microsoft.AspNetCore.Identity;
using Notiflex.Core.Models.DTOs;
using Notiflex.Infrastructure.Data.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.Contracts
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateUserAsync(RegisterDto userDto,  string password);
        Task<bool> IsEmailConfirmedAsync(string email);
        Task<SignInResult> SignInUserAsync(string email, string password);
        Task<bool> UserExistsByEmail(string email);
        Task<string> GetUserIdByEmail(string email);
        Task SignOutAsync();
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<IdentityResult> ResetPasswordAsync(string email, string code, string newPassword);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);
        Task EditProfile(string userId, ProfileDto model);
        Task<NotiflexUser> GetUserData(string userId);
    }
}
