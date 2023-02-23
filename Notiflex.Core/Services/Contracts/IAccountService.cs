using Microsoft.AspNetCore.Identity;
using Notiflex.Core.Models.DTOs;
using Notiflex.Infrastructure.Data.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bots.Types;

namespace Notiflex.Core.Services.Contracts
{
    public interface IAccountService
    {
        /// <summary>
        /// Creates a user with the specified details.
        /// </summary>
        /// <param name="userDto">Given user data.</param>
        /// <param name="password">User password.</param>
        /// <returns>Identity result - success / failure.</returns>
        /// <exception cref="ArgumentException">Invalid data in userDto.</exception>
        Task<IdentityResult> CreateUserAsync(RegisterDto userDto,  string password);

        /// <summary>
        /// Checks if the given email is confirmed.
        /// </summary>
        /// <param name="email">The email to be checked.</param>
        /// <returns>Bool to indicate whether the email is confirmed.</returns>
        /// <exception cref="Notiflex.Core.Exceptions.NotFoundException">Throws if user with that email doesn't exist.</exception>
        Task<bool> IsEmailConfirmedAsync(string email);

        /// <summary>
        /// Signs in the specific user.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>SignInResult - success / fail</returns>
        /// <exception cref="Exceptions.NotFoundException">User to be signed in doesn't exist.</exception>
        Task<SignInResult> SignInUserAsync(string email, string password);

        /// <summary>
        /// Checks if user exists by email.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>Bool whether the user exists.</returns>
        Task<bool> UserExistsByEmail(string email);

        /// <summary>
        /// Gets the userId by the email of the user.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>The id of the user as string.</returns>
        /// <exception cref="Exceptions.NotFoundException">User with that email doesn't exist.</exception>
        Task<string> GetUserIdByEmail(string email);

        /// <summary>
        /// Signs the user out.
        /// </summary>
        Task SignOutAsync();

        /// <summary>
        /// Generates a token used to confirm the email of the user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns></returns>
        /// <exception cref="Exceptions.NotFoundException">User with the given id doesn't exist.</exception>
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);

        /// <summary>
        /// Generates a token used to reset the password of the user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns></returns>
        /// <exception cref="Exceptions.NotFoundException">User with the given id doesn't exist.</exception>
        Task<string> GeneratePasswordResetTokenAsync(string userId);

        /// <summary>
        /// Changes the password of the user with the given new one if the token is valid.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="code">The generated token sent to the user via email.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>IdentityResult - success / fail</returns>
        /// <exception cref="Exceptions.NotFoundException">User with that email doesn't exist.</exception>
        /// <exception cref="ArgumentException">Invalid arguments - code or new password.</exception>
        Task<IdentityResult> ResetPasswordAsync(string email, string code, string newPassword);

        /// <summary>
        /// Confirms the email of the user if the token is valid.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="code">The generated token sent to the user via email.</param>
        /// <returns>IdentityResult - success / fail</returns>
        /// <exception cref="Exceptions.NotFoundException">User with that email doesn't exist.</exception>
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);

        /// <summary>
        /// Edits the profile of the user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="model">Model with new data.</param>
        /// <exception cref="ArgumentException">Invalid ProfileDto model.</exception>
        /// <exception cref="Exceptions.NotFoundException">Invalid userId.</exception>
        Task EditProfile(string userId, ProfileDto model);

        /// <summary>
        /// Gets the saved data of the user with the given id.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>ProfileDto model with all the data</returns>
        /// <exception cref="Exceptions.NotFoundException">User with that id doesnt't exist.</exception>
        Task<ProfileDto> GetUserData(string userId);

        /// <summary>
        /// Approves the user and gives them the approved user role. Sets their telegramId, profilePic and homeTown.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="telegramId">Telegram id of the user.</param>
        /// <param name="hometown">Home town of the user.</param>
        /// <param name="photo">Profile picture of the user.</param>
        /// <exception cref="ArgumentException">Invalid arguments.</exception>
        Task AproveUser(string userId, string telegramId, string hometown, string photo);

        /// <summary>
        /// Checks if the user with the specified id has the specified role.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="roleName">The name of the role</param>
        /// <returns>Bool flag to indicate whether the user has the role. </returns>
        Task<bool> IsInRole(string userId, string roleName);


    }
}
