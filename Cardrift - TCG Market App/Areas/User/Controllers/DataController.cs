using Cardrift___TCG_Market_App.Data;
using Cardrift___TCG_Market_App.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cardrift___TCG_Market_App.Areas.User.Controllers
{
    [Area("user")]
    public class DataController : BaseController
    {
        public DataController(AppDbContext context) : base(context)
        {
        }


        [Route("products/{category}")]
        public IActionResult Product(string category)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.CategoryNow = category;
            var products = _context.Products.Where(x => x.Category.PageName == category).ToList();

            return View(products);
        }

        [Route("products/{game}/{category}")]
        public IActionResult Product(string game, string category)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.CategoryNow = category;
            ViewBag.GameNow = game;

            var products = _context.Products.Where(x => x.Game.PageName == game && x.Category.PageName == category).ToList();

            return View(products);
        }



        [Route("cards/{game}")]
        public IActionResult Card(string game)
        {
            ViewBag.Sets = _context.Sets.Where(x => x.Game.PageName == game).ToList();
            ViewBag.Rarities = _context.Cards.Where(x => x.Game.PageName == game).GroupBy(x => x.Rarity).Select(x => x.Key).ToList();
            ViewBag.Game = game;

            var cards = _context.Cards.Where(x => x.Game.PageName == game).ToList();

            return View(cards);
        }
    }
}
