using Cardrift___TCG_Market_App.Data;
using Cardrift___TCG_Market_App.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cardrift___TCG_Market_App.Areas.User.Controllers
{
    [Area("user")]
    public class MainController : BaseController
    {
        public MainController(AppDbContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            var categories_count = _context.Categories.ToList().Count;
            var products_count = _context.Categories.ToList().Count;

            if(categories_count == 0 || products_count == 0)
            {
                var category = new Category
                {
                    Name = "Card"
                };

                _context.Add(category);
                _context.SaveChanges();

                var card = new Card
                {
                    Product = new Product // Burada Product'ı direkt oluşturuyoruz
                    {
                        Name = "Dark Magician",
                        Description = "The ultimate wizard in terms of attack and defense.",
                        Price = 29.99m,
                        ImageUrl = "dark-magician.jpg",
                        StockQuantity = 100,
                        CategoryId = 1
                    },
                    Set = "Legend of Blue-Eyes White Dragon",
                    Type = 1,
                    Rarity = "Ultra Rare"
                };

                _context.Add(card);
                _context.SaveChanges();
            }

            return View();
        }
    }
}
