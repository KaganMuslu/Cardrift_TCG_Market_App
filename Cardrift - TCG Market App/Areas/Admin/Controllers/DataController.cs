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

        public IActionResult Games()
        {
            return View();
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
