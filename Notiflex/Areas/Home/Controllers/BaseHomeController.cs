using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Notiflex.Areas.Home.Controllers
{
    [Area("Home")]
    [AllowAnonymous]
    public class BaseHomeController : Controller
    {
    }
}
