using Microsoft.AspNetCore.Mvc;

namespace Cardrift___TCG_Market_App.Areas.User.Controllers
{
    [Area("user")]
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
