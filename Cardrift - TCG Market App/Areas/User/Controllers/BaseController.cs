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
            base.OnActionExecuting(filterContext);
            ViewBag.CategoryGame = _context.CategoryGame.Include(x => x.Game).Include(x => x.Category).ToList();
            ViewBag.Games = _context.Games.ToList();
        }
    }
}
