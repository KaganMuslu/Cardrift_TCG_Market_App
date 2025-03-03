using Microsoft.AspNetCore.Mvc;

namespace Cardrift___TCG_Market_App.Areas.User.Controllers
{
    [Area("user")]
    public class ProductController : Controller
    {
        [Route("products")]
        public IActionResult Product()
        {
            return View();
        }

        [Route("products/{category}")]
        public IActionResult Product(string category)
        {
            string key = "2 card found!";

            return View("product", key);
        }

        [Route("products/{category}/{type}")]
        public IActionResult Product(string category, string type)
        {
            string key = "5 card found!";

            return View("product", key);
        }
    }
}
