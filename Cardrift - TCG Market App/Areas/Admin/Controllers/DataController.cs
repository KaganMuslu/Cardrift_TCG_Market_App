using Cardrift___TCG_Market_App.Data;
using Cardrift___TCG_Market_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Cardrift___TCG_Market_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DataController : BaseController
    {
        public DataController(AppDbContext context) : base(context)
        {
        }

        private List<T> PagedData<T>(int page, string? searchTerm) where T : class
        {
            if (page < 1) page = 1; // Sayfa numarası 0 veya negatifse düzelt

            int skip = (page - 1) * 8;

            // Sorgu oluşturur, ancak anında çalıştırmaz
            IQueryable<T> query = _context.Set<T>(); // Dinamik tablo seçimi

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => EF.Property<string>(x, "Name").Contains(searchTerm)); // Dinamik arama
            }

            List<T> data = query.Skip(skip).Take(8).ToList(); // Sayfalama

            // Sonraki sayfa kontrolü
            bool hasNext = query.Skip(page * 8).Any();

            ViewBag.next = hasNext;
            ViewBag.page = page;

            return data;
        }



        #region Game Section

        public IActionResult Games(int page, string? searchTerm)
        {
            var games = PagedData<Game>(page, searchTerm);


            return View(games);
        }

        public IActionResult AddGame()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddGame(Game game, IFormFile ImageFile)
        {
            if (_context.Games.FirstOrDefault(x => x.Name == game.Name) != null)
            {
                ModelState.AddModelError("Name", "There is already a game with this name!");
            }
            else
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Dosya yüklendiyse, ImageUrl'yi güncelle
                    Random rnd = new Random();

                    string imageName = ImageFile.FileName;
                    string extension = Path.GetExtension(imageName);

                    string str = "abcdefghijklmnopqrstuvwxyz0123456789";
                    string fileName = "";

                    for (int i = 0; i < 10; i++)
                    {
                        int x = rnd.Next(str.Length);
                        fileName = fileName + str[x];
                    }
                    fileName += extension;

                    var filePath = Path.Combine("wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    // URL'yi güncelle
                    game.ImageUrl = "/images/" + fileName;
                }
            }

            ModelState.Remove("ImageFile");

            if (ModelState.IsValid)
            {
                _context.Add(game);
                _context.SaveChanges();

                return RedirectToAction("games");
            }

            return View(game);
        }

        public IActionResult EditGame(int id)
        {
            var editGame = _context.Games.FirstOrDefault(x => x.Id == id);
            if (editGame == null)
            {
                return RedirectToAction("games");
            }

            return View(editGame);
        }

        [HttpPost]
        public IActionResult EditGame(Game game, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Dosya yüklendiyse, ImageUrl'yi güncelle
                Random rnd = new Random();

                string imageName = ImageFile.FileName;
                string extension = Path.GetExtension(imageName);

                string str = "abcdefghijklmnopqrstuvwxyz0123456789";
                string fileName = "";

                for (int i = 0; i < 10; i++)
                {
                    int x = rnd.Next(str.Length);
                    fileName = fileName + str[x];
                }
                fileName += extension;

                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                // URL'yi güncelle
                game.ImageUrl = "/images/" + fileName;
            }

            ModelState.Remove("ImageFile");

            if (ModelState.IsValid)
            {
                _context.Update(game);
                _context.SaveChanges();

                return RedirectToAction("games"); ;
            }

            return View(game);
        }

        [HttpPost]
        public IActionResult DeleteGame(int id)
        {
            var deleteGame = _context.Games.FirstOrDefault(x => x.Id == id);
            if (deleteGame != null)
            {
                _context.Remove(deleteGame);
                _context.SaveChanges();
            }

            return RedirectToAction("games");
        }

        #endregion

        #region Products Section

        public IActionResult Products(int page, string? searchTerm)
        {
            var products = PagedData<Product>(page, searchTerm);


            return View(products);
        }

        #endregion

        #region Cards Section

        public IActionResult Cards()
        {
            return View();
        }

        #endregion

        

        public IActionResult Categories()
        {
            return View();
        }


    }
}
