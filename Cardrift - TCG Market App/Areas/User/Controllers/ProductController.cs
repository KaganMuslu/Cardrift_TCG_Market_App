using Cardrift___TCG_Market_App.Data;
using Microsoft.AspNetCore.Mvc;

namespace Cardrift___TCG_Market_App.Areas.User.Controllers
{
    [Area("user")]
    public class ProductController : BaseController
    {
        public ProductController(AppDbContext context) : base(context)
        {
        }

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
            var products = _context.Products.ToList();

            return View(products);
        }
    }
}   
