using Cardrift___TCG_Market_App.Data;
using Cardrift___TCG_Market_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Linq.Expressions;

namespace Cardrift___TCG_Market_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DataController : BaseController
    {
        public DataController(AppDbContext context) : base(context)
        {
        }

        // Pagedli sayfalama
        private List<T> PagedData<T>(int page, string? searchTerm, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (page < 1) page = 1; // Sayfa numarası 0 veya negatifse düzelt

            int skip = (page - 1) * 8;

            // Sorgu oluşturur, ancak anında çalıştırmaz
            IQueryable<T> query = _context.Set<T>(); // Dinamik tablo seçimi

            // Include edilen navigation property'leri ekle
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => EF.Property<string>(x, "Name").Contains(searchTerm)); // Dinamik arama
            }

            List<T> data = query.OrderBy(x => EF.Property<object>(x, "Id"))
                .Skip(skip)
                .Take(8)
                .ToList(); // Sayfalama

            // Sonraki sayfa kontrolü
            bool hasNext = query.Skip(page * 8).Any();

            ViewBag.next = hasNext;
            ViewBag.page = page;

            return data;
        }

        // Rastgele resim ismi üretme ve kaydetme
        private string UniqueFileNameCopy(IFormFile ImageFile)
        {
            string originalFileName = ImageFile.FileName;

            Random rnd = new Random();
            string extension = Path.GetExtension(originalFileName);

            string str = "abcdefghijklmnopqrstuvwxyz0123456789";
            string fileName = "";

            for (int i = 0; i < 10; i++)
            {
                int x = rnd.Next(str.Length);
                fileName += str[x];
            }

            var filePath = Path.Combine("wwwroot/images", fileName + extension);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                ImageFile.CopyTo(stream);
            }

            return fileName + extension;
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
            ModelState.Remove("ImageFile");

            if (!ModelState.IsValid)
            {
                return View(game);
            }

            var sameName = _context.Games.Where(x => x.Name == game.Name && x.Id != game.Id).FirstOrDefault();
            if (sameName != null)
            {
                ModelState.AddModelError("Name", "This name is already in use!");
                return View(game);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = UniqueFileNameCopy(ImageFile);

                // URL'yi güncelle
                game.ImageUrl = "/images/" + fileName;
            }

            _context.Add(game);
            _context.SaveChanges();

            return RedirectToAction("games");

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
            ModelState.Remove("ImageFile");

            if (!ModelState.IsValid)
            {
                return View(game);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = UniqueFileNameCopy(ImageFile);

                // URL'yi güncelle
                game.ImageUrl = "/images/" + fileName;
            }

            _context.Update(game);
            _context.SaveChanges();

            return RedirectToAction("games"); ;
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

        public IActionResult AddProduct()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Games = _context.Games.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product, IFormFile ImageFile)
        {
            ModelState.Remove("ImageFile");
            ModelState.Remove("Category");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Games = _context.Games.ToList();

                return View(product);
            }

            var sameName = _context.Products.Where(x => x.Name == product.Name && x.Id != product.Id).FirstOrDefault();
            if (sameName != null)
            {
                ModelState.AddModelError("Name", "This name is already in use!");

                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Games = _context.Games.ToList();

                return View(product);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = UniqueFileNameCopy(ImageFile);

                // URL'yi güncelle
                product.ImageUrl = "/images/" + fileName;
            }

            _context.Add(product);
            _context.SaveChanges();

            return RedirectToAction("products");
        }

        public IActionResult EditProduct(int id)
        {
            var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();

            if (product == null)
            {
                return RedirectToAction("products");
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Games = _context.Games.ToList();

            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product, IFormFile ImageFile)
        {
            ModelState.Remove("ImageFile");
            ModelState.Remove("Category");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Games = _context.Games.ToList();

                return View(product);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = UniqueFileNameCopy(ImageFile);

                // URL'yi güncelle
                product.ImageUrl = "/images/" + fileName;
            }

            _context.Update(product);
            _context.SaveChanges();

            return RedirectToAction("products");
        }

        public IActionResult DeleteProduct(int id)
        {
            var deleteProduct = _context.Products.FirstOrDefault(x => x.Id == id);

            if(deleteProduct != null)
            {
                _context.Remove(deleteProduct);
                _context.SaveChanges();
            }

            return RedirectToAction("products");
        }

        #endregion

    #region Cards Section

        public IActionResult Cards(int page, string? searchTerm)
        {
            var cards = PagedData<Card>(page, searchTerm, x => x.Set);

            return View(cards);
        }

        public IActionResult AddCard()
        {
            ViewBag.Games = _context.Games.ToList();
            ViewBag.Sets = _context.Sets.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult AddCard(Card card, IFormFile ImageFile)
        {
            ModelState.Remove("ImageFile");
            ModelState.Remove("Game");
            ModelState.Remove("Set");

            if (!ModelState.IsValid)
            {
                ViewBag.Games = _context.Games.ToList();
                ViewBag.Sets = _context.Sets.ToList();

                return View(card);
            }

            var sameName = _context.Cards.Where(x => x.Name == card.Name && card.SetId == x.SetId && card.Rarity == x.Rarity).FirstOrDefault();
            if (sameName != null)
            {
                ModelState.AddModelError("Name", "This card is already exists!");

                ViewBag.Games = _context.Games.ToList();
                ViewBag.Sets = _context.Sets.ToList();

                return View(card);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = UniqueFileNameCopy(ImageFile);

                // URL'yi güncelle
                card.ImageUrl = "/images/" + fileName;
            }

            _context.Add(card);
            _context.SaveChanges();

            return RedirectToAction("cards");
        }

        public IActionResult EditCard(int id)
        {
            var card = _context.Cards.Where(x => x.Id == id).FirstOrDefault();

            if (card == null)
            {
                return RedirectToAction("cards");
            }

            ViewBag.Games = _context.Games.ToList();
            ViewBag.Sets = _context.Sets.ToList();

            return View(card);
        }

        [HttpPost]
        public IActionResult EditCard(Card card, IFormFile ImageFile)
        {
            ModelState.Remove("ImageFile");
            ModelState.Remove("Game");
            ModelState.Remove("Set");

            if (!ModelState.IsValid)
            {
                ViewBag.Games = _context.Games.ToList();
                ViewBag.Sets = _context.Sets.ToList();

                return View(card);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = UniqueFileNameCopy(ImageFile);

                // URL'yi güncelle
                card.ImageUrl = "/images/" + fileName;
            }

            _context.Update(card);
            _context.SaveChanges();

            return RedirectToAction("cards");
        }

        public IActionResult DeleteCard(int id)
        {
            var deleteCard = _context.Cards.FirstOrDefault(x => x.Id == id);

            if (deleteCard != null)
            {
                _context.Remove(deleteCard);
                _context.SaveChanges();
            }

            return RedirectToAction("cards");
        }


    #endregion


    #region Categories Section

        public IActionResult Categories(int page, string? searchTerm)
        {
            var categories = PagedData<Category>(page, searchTerm);

            return View(categories);
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var sameName = _context.Categories.Where(x => x.Name == category.Name).FirstOrDefault();
            if (sameName != null)
            {
                ModelState.AddModelError("Name", "This category is already exists!");

                return View(category);
            }

            _context.Add(category);
            _context.SaveChanges();

            return RedirectToAction("categories");
        }

        public IActionResult EditCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);

            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Update(category);
            _context.SaveChanges();

            return RedirectToAction("categories");
        }

        public IActionResult DeleteCategory(int id)
        {
            var deleteCategory = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (deleteCategory != null)
            {
                _context.Remove(deleteCategory);
                _context.SaveChanges();
            }

            return RedirectToAction("categories");
        }

        #endregion

        #region Sets Section

        public IActionResult Sets()
        {
            return View();
        }


    #endregion


    }
}
