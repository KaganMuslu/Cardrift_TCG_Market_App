using Cardrift___TCG_Market_App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Cardrift___TCG_Market_App.Areas.User.Controllers
{
    public class BaseController : Controller
    {
        protected readonly AppDbContext _context;

        public BaseController(AppDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Games = _context.Games.ToList();
            ViewBag.CategoryGame = _context.CategoryGames
                .Include(cg => cg.Game)
                .Include(cg => cg.Category)
                .ToList();
            ViewBag.Supplies = _context.Categories
                .Where(x => !x.CategoryGames.Any(c => c.CategoryId == x.Id))
                .ToList();
        }
    }
}
