using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notiflex.Core.Models;
using Notiflex.Core.Models.HomePageModels;
using Notiflex.Core.Services.Contracts;
using Notiflex.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Telegram.Bot.Types;

namespace Notiflex.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessageSender _messageSender;
        private readonly IAccountService _accountService;
        private readonly IModelConfigurer _modelConfigurer;

        public HomeController(IMessageSender messageSender, IAccountService accountService, IModelConfigurer modelConfigurer)
        {
            _messageSender = messageSender;
            _accountService = accountService;
            _modelConfigurer = modelConfigurer;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //[Authorize]
        //public IActionResult Browse()
        //{
        //    List<IndexModel> model = new()
        //    {
        //        new IndexModel()
        //        {
        //            Avalable = false
        //        }
        //    };

        //    return View(model);
        //}

        //[Authorize]
        //public async Task<IActionResult> SendData(string id)
        //{  
        //    await _messageSender.SendMessage("Test message", id);

        //    return RedirectToAction(nameof(Index));
        //}

        //[Authorize]
        //public async Task<IActionResult> Browse(string value)
        //{
        //    try
        //    {
        //        if ((await _messageSender.ConvertNameToCoordinates(value)) == null)
        //        {
        //            return BadRequest();
        //        }

        //        List<IndexModel> model = await _modelConfigurer.ConfigureForecastReport(value);

        //        if (User?.Identity?.IsAuthenticated ?? false)
        //        {
        //            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        //            string chatId = (await _accountService.GetUserData(userId)).TelegramInfo;

        //            if (chatId == null)
        //            {

        //            }
        //            else
        //            {
        //                StringBuilder sb = new();
        //                sb.AppendLine($"Today's average temperature for {model.First().Name} is -> {model.First().Temp}C°");

        //                await _messageSender.SendMessage(sb.ToString(), chatId);
        //            }
        //        }

        //        return View(model);
        //    }
        //    catch(Exception)
        //    {
        //        return View();
        //    }
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}