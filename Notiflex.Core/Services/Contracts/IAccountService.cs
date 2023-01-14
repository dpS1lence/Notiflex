using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.Contracts
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateUserAsync(string email, string firstName, string lastName, string userName, string password);
        Task<bool> IsEmailConfirmedAsync(string email);
        Task<SignInResult> SignInUserAsync(string email, string password);
        Task<bool> UserExistsByEmail(string email);
        Task<string> GetUserIdByEmail(string email);
        Task SignOutAsync();
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<IdentityResult> ResetPasswordAsync(string email, string code, string password);
    }
}
