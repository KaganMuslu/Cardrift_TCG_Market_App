using Cardrift___TCG_Market_App.Data;
using Cardrift___TCG_Market_App.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cardrift___TCG_Market_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DataController : BaseController
    {
        public DataController(AppDbContext context) : base(context)
        {
        }

        public IActionResult Games(int page)
        {
            if (page == 0) page = 1;
            ViewBag.page = page;

            int skip = (page - 1) * 8;
            var games = _context.Games.Skip(skip).Take(8).ToList();

            int nextSkip = page * 8;
            var next = _context.Games.Skip(nextSkip).Count();

            if (next > 0)
            {
                ViewBag.next = true;
            }
            else
            {
                ViewBag.next = false;
            }

            return View(games);
        }

        public IActionResult AddGame()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddGame(Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                _context.SaveChanges();

                return RedirectToAction("games");
            }

            return View(game);
            
        }


        public IActionResult Cards()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        public IActionResult Categories()
        {
            return View();
        }


    }
}
