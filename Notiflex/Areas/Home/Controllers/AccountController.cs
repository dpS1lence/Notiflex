using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Notiflex.Core.Models;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Services.AccountServices;
using Notiflex.Core.Services.BOtServices;
using Notiflex.Core.Services.Contracts;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.Infrastructure.Repositories.Contracts;
using Notiflex.ViewModels;
using System.Security.Claims;
using System.Text;

namespace Notiflex.Areas.Home.Controllers
{
    public class AccountController : BaseHomeController
    {
        private readonly IAccountService _accountService;
        private readonly IEmailSender _emailSender;
        private readonly IModelConfigurer _modelConfigurer;
        private readonly IMapper _mapper;
        public AccountController(IModelConfigurer modelConfigurer, IAccountService accountService, IEmailSender emailSender, IMapper mapper)
        {
            _accountService = accountService;
            _emailSender = emailSender;
            _mapper = mapper;
            _modelConfigurer = modelConfigurer;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.IsInRole("ApprovedUser"))
            {
                return RedirectToAction("Dashboard", "Dashboard", new { area = "Main" });
            }

            var model = new RegisterViewModel();

            return PartialView(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState.Root.Children)
                {
                    if(item.ValidationState == ModelValidationState.Invalid)
                    {
                        TempData["StatusMessageDanger"] = $"{item.AttemptedValue} is invalid";
                    }
                }
                return PartialView(model);
            }
            RegisterDto userDto;
            try
            {
                userDto = _mapper.Map<RegisterDto>(model);
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
                return RedirectToAction("Login", "Account");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return PartialView(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.IsInRole("ApprovedUser"))
            {
                return RedirectToAction("Dashboard", "Dashboard", new { area = "Main" });
            }

            var model = new LoginViewModel();

            return PartialView(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState.Root.Children)
                {
                    if (item.ValidationState == ModelValidationState.Invalid)
                    {
                        TempData["StatusMessageDanger"] = $"{item.AttemptedValue} is invalid";
                    }
                }

                return PartialView(model);
            }
            if (!await _accountService.IsEmailConfirmedAsync(model.Email))
            {
                TempData["StatusMessage"] = "Email not confirmed!";
                return PartialView(model);
            }
            var signInResult = await _accountService.SignInUserAsync(model.Email, model.Password);

            if (signInResult.Succeeded)
            {
                string userId = await _accountService.GetUserIdByEmail(model.Email);
                var user = await _accountService.GetUserData(userId);

                if (!await _accountService.IsInRole(userId, "ApprovedUser"))
                {
                    return RedirectToAction(nameof(Proceed));
                }

                return RedirectToAction("Dashboard", "Dashboard", new { area = "Main" });
            }

            ModelState.AddModelError("", "Invalid Login");

            return PartialView(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOutAsync();

            return RedirectToAction("Login", "Account", new { area = "Home" });

        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string? userId, string? token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            IdentityResult result = await _accountService.ConfirmEmailAsync(userId, token);

            TempData["StatusMessage"] = result.Succeeded ? "Thank you for confirming your email." : "An error occurred while trying to confirm your email";

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Proceed()
        {
            if (User.IsInRole("ApprovedUser"))
            {
                return RedirectToAction("Profile", "Dashboard", new { area = "Main" });
            }

            return PartialView();
        }

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Proceed(ProceedViewModel model)
		{
            if (!ModelState.IsValid)
			{
				return PartialView(model);
			}

            if ((await _modelConfigurer.ConvertNameToCoordinates(model.HomeTown))[2] == null)
            {
                TempData["StatusMessageDanger"] = "Invalid city name!";

                return RedirectToAction(nameof(Proceed));
            }

            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value!;
            await _accountService.AprooveUser(userId, model.TelegramId, model.HomeTown);

            return RedirectToAction("Logout", "Account", new { area = "Home" });
		}
    }
}
