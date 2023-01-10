using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notiflex.Core.Models;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.Infrastructure.Repositories.Contracts;

namespace Notiflex.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<NotiflexUser> userManager;
        private readonly SignInManager<NotiflexUser> signInManager;
        private readonly IRepository repo;
        public UserController(UserManager<NotiflexUser> _userManager, SignInManager<NotiflexUser> _signInManager, IRepository _repo)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            repo = _repo;
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

            var user = new NotiflexUser()
            {
                Email = model.Email,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                HomeTown = model.HomeTown,
                DefaultTime = model.DefaultTime,
                TelegramInfo = model.TelegramInfo,
                ProfilePic = model.ProfilePic,
                Gender = model.Gender,
                Age = model.Age,
                Description = model.Description
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "User");
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

            var user = await userManager.FindByNameAsync(model.UserName);

            if (!user.Equals(null))
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid Login");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}
