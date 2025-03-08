using Cardrift___TCG_Market_App.Data;
using Microsoft.AspNetCore.Mvc;

namespace Cardrift___TCG_Market_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DataController : BaseController
    {
        public DataController(AppDbContext context) : base(context)
        {
        }

        [Route("admin/data/{category}")]
        public IActionResult Data(string category)
        {
            if (category == "all-games")
            {
                var games = _context.Games.ToList();
            }

            return View();
        }
    }
}
