﻿using Cardrift___TCG_Market_App.Data;
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
        private List<T> PagedData<T>(int page, string? searchTerm, string? game, string? category, string? set, string? rarity, params Expression<Func<T, object>>[] includes) where T : class
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

            // Eğer product sınıfı üzerine çalışıyorsak yapılacak filtrelemeler
            if (typeof(T) == typeof(Product))
            {
                if (!string.IsNullOrEmpty(game))
                {
                    query = query.Where(x => ((Product)(object)x).Game.Name == game);
                }

                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(x => ((Product)(object)x).Category.Name == category);
                }
            }

            // Card modeli için filtreleme
            if (typeof(T) == typeof(Card))
            {
                if (!string.IsNullOrEmpty(game))
                {
                    query = query.Where(x => ((Card)(object)x).Game.Name == game);
                }

                if (!string.IsNullOrEmpty(set))
                {
                    query = query.Where(x => ((Card)(object)x).Set.Name == set);
                }

                if (!string.IsNullOrEmpty(rarity))
                {
                    query = query.Where(x => ((Card)(object)x).Rarity == rarity);
                }
            }

            // Set modeli için filtreleme
            if (typeof(T) == typeof(Set))
            {
                if (!string.IsNullOrEmpty(game))
                {
                    query = query.Where(x => ((Set)(object)x).Game.Name == game);
                }
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
            var games = PagedData<Game>(page, searchTerm, null, null, null, null);


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

            var sameName = _context.Games.Where(x => x.Name == game.Name).FirstOrDefault();
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

            game.PageName = new string(game.Name
                .Where(c => Char.IsLetterOrDigit(c))    // Yalnızca harf ve rakamları al
                .ToArray())
                .ToLower();

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

            // Önceden eklenmiş aynı addakileri kontrol
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

            game.PageName = new string(game.Name
                .Where(c => Char.IsLetterOrDigit(c))    // Yalnızca harf ve rakamları al
                .ToArray())
                .ToLower();

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

        public IActionResult Products(int page, string? searchTerm, string? game, string? category)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Games = _context.Games.ToList();

            var products = PagedData<Product>(page, searchTerm, game, category, null, null, x => x.Game, y => y.Category);
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
            ModelState.Remove("GameId");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Games = _context.Games.ToList();

                return View(product);
            }

            var sameName = _context.Products.Where(x => x.Name == product.Name).FirstOrDefault();
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


            // Ardından CategoryGame ilişkisini kontrol et
            var exists = _context.CategoryGames
                .Any(cg => cg.CategoryId == product.CategoryId && cg.GameId == product.GameId);

            if (product.GameId.HasValue && !exists)
            {
                _context.CategoryGames.Add(new CategoryGame
                {
                    CategoryId = product.CategoryId,
                    GameId = product.GameId.Value
                });

                _context.SaveChanges();
            }


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

            var sameName = _context.Products
                .FirstOrDefault(x => x.Name == product.Name && x.Id != product.Id);

            if (sameName != null)
            {
                ModelState.AddModelError("Name", "This name is already in use!");
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Games = _context.Games.ToList();
                return View(product);
            }

            // ⬇️ Eski değerleri al (silme kontrolü için)
            var existingProduct = _context.Products
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == product.Id);

            if (existingProduct == null)
                return RedirectToAction("products");

            var oldCategoryId = existingProduct.CategoryId;
            var oldGameId = existingProduct.GameId;

            // 📷 Resim işlemi
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = UniqueFileNameCopy(ImageFile);
                product.ImageUrl = "/images/" + fileName;
            }

            _context.Update(product);

            // ✅ Yeni ilişkiyi kontrol et ve ekle
            if (product.GameId.HasValue)
            {
                bool newRelationExists = _context.CategoryGames
                    .Any(cg => cg.CategoryId == product.CategoryId && cg.GameId == product.GameId);

                if (!newRelationExists)
                {
                    _context.CategoryGames.Add(new CategoryGame
                    {
                        CategoryId = product.CategoryId,
                        GameId = product.GameId.Value
                    });
                }
            }

            // ❌ Eski ilişki artık kullanılmıyorsa sil
            if (oldGameId.HasValue &&
                (oldCategoryId != product.CategoryId || oldGameId != product.GameId))
            {
                bool isOldRelationStillUsed = _context.Products.Any(p =>
                    p.Id != product.Id &&
                    p.CategoryId == oldCategoryId &&
                    p.GameId == oldGameId);

                if (!isOldRelationStillUsed)
                {
                    var oldRelation = _context.CategoryGames
                        .FirstOrDefault(cg => cg.CategoryId == oldCategoryId && cg.GameId == oldGameId.Value);

                    if (oldRelation != null)
                    {
                        _context.CategoryGames.Remove(oldRelation);
                    }
                }
            }

            _context.SaveChanges(); // ✅ tek seferde kayıt
            return RedirectToAction("products");
        }


        public IActionResult DeleteProduct(int id)
        {
            var deleteProduct = _context.Products.FirstOrDefault(x => x.Id == id);

            if (deleteProduct != null)
            {
                // İlişki kontrolü için önce CategoryId ve GameId'yi alıyoruz
                var oldCategoryId = deleteProduct.CategoryId;
                var oldGameId = deleteProduct.GameId;

                // Ürünü siliyoruz
                _context.Remove(deleteProduct);
                _context.SaveChanges();

                // Bu ilişkiyi hâlâ kullanan başka ürün var mı?
                bool isOldRelationStillUsed = _context.Products
                    .Any(p => p.CategoryId == oldCategoryId && p.GameId == oldGameId);

                // Yoksa CategoryGame ilişkisini de silelim
                if (!isOldRelationStillUsed && oldGameId != null)
                {
                    var relation = _context.CategoryGames
                        .FirstOrDefault(cg => cg.CategoryId == oldCategoryId && cg.GameId == oldGameId.Value);

                    if (relation != null)
                    {
                        _context.CategoryGames.Remove(relation);
                        _context.SaveChanges();
                    }
                }
            }

            return RedirectToAction("products");
        }


        #endregion

    #region Cards Section

        public IActionResult Cards(int page, string? searchTerm, string? game, string? set, string? rarity)
        {
            ViewBag.Games = _context.Games.ToList();
            ViewBag.Sets = _context.Sets.Where(x => x.Game.PageName == game).ToList();
            ViewBag.Rarities = _context.Cards.Where(x => x.Game.PageName == game).GroupBy(x => x.Rarity).Select(x => x.Key).ToList();

            var cards = PagedData<Card>(page, searchTerm, game, null, set, rarity, x => x.Set, y => y.Game);

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

            var sameName = _context.Cards.Where(x => x.Name == card.Name && card.SetId == x.SetId && card.GameId == x.GameId && card.Rarity == x.Rarity).FirstOrDefault();
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

            var sameName = _context.Cards.Where(x => x.Name == card.Name && card.SetId == x.SetId && card.GameId == x.GameId && card.Rarity == x.Rarity && card.Id != x.Id).FirstOrDefault();
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
            var categories = PagedData<Category>(page, searchTerm, null, null, null, null);

            return View(categories);
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            ModelState.Remove("PageName");
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

            category.PageName = new string(category.Name
                .Where(c => Char.IsLetterOrDigit(c))    // Yalnızca harf ve rakamları al
                .ToArray())
                .ToLower();

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

            // Önceden eklenmiş aynı addakileri kontrol
            var sameName = _context.Categories.Where(x => x.Name == category.Name && x.Id != category.Id).FirstOrDefault();
            if (sameName != null)
            {
                ModelState.AddModelError("Name", "This category is already exists!");

                return View(category);
            }

            category.PageName = new string(category.Name
                .Where(c => Char.IsLetterOrDigit(c))    // Yalnızca harf ve rakamları al
                .ToArray())
                .ToLower();

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

        public IActionResult Sets(int page, string? searchTerm, string? game)
        {
            ViewBag.Games = _context.Games.ToList();

            var sets = PagedData<Set>(page, searchTerm, game, null, null, null, x => x.Game);

            return View(sets);
        }

        public IActionResult AddSet()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSet(Set set)
        {
            if (!ModelState.IsValid)
            {
                return View(set);
            }

            var sameName = _context.Sets.Where(x => x.Name == set.Name).FirstOrDefault();
            if (sameName != null)
            {
                ModelState.AddModelError("Name", "This set is already exists!");

                return View(set);
            }

            _context.Add(set);
            _context.SaveChanges();

            return RedirectToAction("sets");
        }

        public IActionResult EditSet(int id)
        {
            var set = _context.Sets.FirstOrDefault(x => x.Id == id);

            if (set == null)
            {
                return RedirectToAction("sets");
            }

            return View(set);
        }

        [HttpPost]
        public IActionResult EditSet(Set set)
        {
            if (!ModelState.IsValid)
            {
                return View(set);
            }

            // Önceden eklenmiş aynı addakileri kontrol
            var sameName = _context.Sets.Where(x => x.Name == set.Name && x.Id != set.Id).FirstOrDefault();
            if (sameName != null)
            {
                ModelState.AddModelError("Name", "This set is already exists!");

                return View(set);
            }

            _context.Update(set);
            _context.SaveChanges();

            return RedirectToAction("sets");
        }

        public IActionResult DeleteSet(int id)
        {
            var deleteSet = _context.Sets.FirstOrDefault(x => x.Id == id);

            if (deleteSet != null)
            {
                _context.Remove(deleteSet);
                _context.SaveChanges();
            }

            return RedirectToAction("sets");
        }


        #endregion

    #region Communication

        public IActionResult ReportError()
        {
            return View();
        }

        public IActionResult AskDeveloper()
        {
            return View();
        }

        #endregion

        #region User Updates

        public IActionResult AllUsers()
        {
            return View();
        }

        public IActionResult UsersTransactions()
        {
            return View();
        }

        #endregion
    }
}
