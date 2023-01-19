using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Notiflex.Core.Models;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Services.AccountServices;
using Notiflex.Core.Services.Contracts;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.Infrastructure.Repositories.Contracts;
using Notiflex.ViewModels;
using System.Security.Claims;
using System.Text;

namespace Notiflex.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        public AccountController(IAccountService accountService, IEmailSender emailSender, IMapper mapper)
        {
            _accountService = accountService;
            _emailSender = emailSender;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            UserDto userDto;
            try
            {
                userDto = _mapper.Map<UserDto>(model);

            }
            catch (Exception)
            {

                throw;
            }
            var result = await _accountService.CreateUserAsync(userDto, model.Password);

            if (result.Succeeded)
            {
                string userId = await _accountService.GetUserIdByEmail(model.Email);
                string token = await _accountService.GenerateEmailConfirmationTokenAsync(userId);
                string callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId, token }, Request.Scheme)!;

                var sb = new StringBuilder();

                sb.AppendLine(callbackUrl);
                await _emailSender.SendEmailAsync(model.Email, "Email Confirmation for Notiflex", sb.ToString());

                //TODO: Send email confirmation
                return RedirectToAction("Index", "Home");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!await _accountService.IsEmailConfirmedAsync(model.Email))
            {
                TempData["StatusMessage"] = "Email not confirmed!";
                return View(model);
            }
            var signInResult = await _accountService.SignInUserAsync(model.Email, model.Password);

            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid Login");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string? userId, string? token)
        {
            if (userId == null || token == null)
            {
                return this.RedirectToAction("Index", "Home");
            }
            IdentityResult result = await _accountService.ConfirmEmailAsync(userId, token);
            TempData["StatusMessage"] = result.Succeeded ? "Thank you for confirming your email." : "An error occurred while trying to confirm your email";
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

            NotiflexUser user = await _accountService.GetUserData(userId);

            var model = new ProfileViewModel()
            {
                TelegramChatId = user?.TelegramInfo ?? "ChatId"
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

            ProfileDto dto = new()
            {
                TelegramChatId = model.TelegramChatId
            };

            await _accountService.EditProfile(userId, dto);

            ProfileViewModel prModel = new()
            {
                TelegramChatId = dto.TelegramChatId
            };

            return View(prModel);
        }
    }
}
