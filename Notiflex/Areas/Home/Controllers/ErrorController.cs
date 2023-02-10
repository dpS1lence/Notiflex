using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notiflex.Core.Exceptions;
using Notiflex.ViewModels;
using System.Diagnostics;

namespace Notiflex.Areas.Home.Controllers
{
    [AllowAnonymous]
    [Area("Home")]
    public class ErrorController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            Exception error = exceptionHandlerPathFeature?.Error!;
            switch (error)
            {
                case NotFoundException:
                    return RedirectToAction("ErrorPage", new { statusCode = 404 });
                case ArgumentException:
                    return RedirectToAction("ErrorPage", new { statusCode = 500 });

                default: return RedirectToAction("ErrorPage", new { statusCode = 500 });
            }
        }
        public IActionResult ErrorPage(int statusCode)
        {
            ViewData["statusCode"] = statusCode;
            return View();
        }
    }
}
