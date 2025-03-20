using Cardrift___TCG_Market_App.Data;
using Cardrift___TCG_Market_App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            var games_count = _context.Games.ToList().Count;

            if (categories_count == 0 || products_count == 0 || games_count == 0)
            {
                var game = new Game
                {
                    Name = "Yu-Gi-Oh!",
                    ImageUrl = "https://logodix.com/logo/1996523.jpg"
                };
                var game2 = new Game
                {
                    Name = "Magic: The Gathering",
                    ImageUrl = "https://gimgs2.nohat.cc/thumb/f/640/magic-the-gathering-logo-google-search-magic-the-gathering-mtg--comdlpng6952219.jpg"
                };
                var game3 = new Game
                {
                    Name = "Digimon",
                    ImageUrl = "https://i.etsystatic.com/15285521/r/il/ab1357/2040468149/il_fullxfull.2040468149_lx9n.jpg"
                };
                var game4 = new Game
                {
                    Name = "Pokemon",
                    ImageUrl = "https://logos-world.net/wp-content/uploads/2020/05/Pokemon-Symbol.jpg"
                };
                var game5 = new Game
                {
                    Name = "Cardfight: Vanguard",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/1/18/Vg_new_logo.png"
                };
                var game6 = new Game
                {
                    Name = "Marvel HeroClix",
                    ImageUrl = "https://www.nicepng.com/png/detail/306-3067793_guardians-of-the-galaxy-heroclix-now-available-age.png"
                };

                var category = new Category
                {
                    Name = "Card"
                };

                _context.Add(game);
                _context.Add(game2);
                _context.Add(game3);
                _context.Add(game4);
                _context.Add(game5);
                _context.Add(game6);
                _context.Add(category);
                _context.SaveChanges();

                var product = new Product
                {
                    Name = "Structure Deck: Blue-Eyes White Destiny",
                    Description = "Prepare yourself for the next chapter of the Blue-Eyes White Dragon strategy: The electrifyingly awesome Blue-Eyes White Destiny Structure Deck!",
                    Price = 11.9m,
                    ImageUrl = "https://www.yugioh-card.com/en/wp-content/uploads/2024/10/SDWD_550.png",
                    StockQuantity = 36,
                    GameId = 2,
                    CategoryId = 1
                };

                _context.Add(product);

                var card = new Card
                {
                    Name = "Ukiyoe-P.U.N.K. Amazing Dragon",
                    Price = 0.63m,
                    ImageUrl = "https://images.ygoprodeck.com/images/cards/44708154.jpg",
                    StockQuantity = 21,
                    GameId = 1,
                    Set = "Battles of Legend: Monstrous Revenge",
                    Type = 1,
                    Rarity = "Secret Rare"
                };
                _context.Add(card);
                _context.SaveChanges();
            }

            return View();
        }
    }
}
