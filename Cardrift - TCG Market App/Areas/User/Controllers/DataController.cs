using Cardrift___TCG_Market_App.Data;
using Microsoft.AspNetCore.Mvc;

namespace Cardrift___TCG_Market_App.Areas.User.Controllers
{
    [Area("user")]
    public class DataController : BaseController
    {
        public DataController(AppDbContext context) : base(context)
        {
        }


        [Route("products")]
        public IActionResult Product()
        {
            var products = _context.Products.ToList();

            return View(products);
        }

        [Route("products/{category}")]
        public IActionResult Product(string category)
        {
            var products = _context.Products.ToList();

            return View("product");
        }

        [Route("products/{game}/{category}")]
        public IActionResult Product(string game, string category)
        {
            var products = _context.Products.Where(x => x.Game.PageName == game && x.Category.PageName == category).ToList();

            return View(products);
        }



        [Route("cards/{game}")]
        public IActionResult Card(string game)
        {
            ViewBag.Sets = _context.Sets.Where(x => x.Game.PageName == game).ToList();
            ViewBag.Rarities = _context.Cards.Where(x => x.Game.PageName == game).GroupBy(x => x.Rarity).Select(x => x.Key).ToList();

            var cards = _context.Cards.Where(x => x.Game.PageName == game).ToList();

            return View(cards);
        }
    }
}
