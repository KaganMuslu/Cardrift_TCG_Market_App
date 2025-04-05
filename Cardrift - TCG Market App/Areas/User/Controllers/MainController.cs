using Cardrift___TCG_Market_App.Data;
using Cardrift___TCG_Market_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    PageName = "YuGiOh",
                    ImageUrl = "https://logodix.com/logo/1996523.jpg"
                };
                var game2 = new Game
                {
                    Name = "Magic: The Gathering",
                    PageName = "MagicTheGathering",
                    ImageUrl = "https://gimgs2.nohat.cc/thumb/f/640/magic-the-gathering-logo-google-search-magic-the-gathering-mtg--comdlpng6952219.jpg"
                };
                var game3 = new Game
                {
                    Name = "Digimon",
                    PageName = "Digimon",
                    ImageUrl = "https://i.etsystatic.com/15285521/r/il/ab1357/2040468149/il_fullxfull.2040468149_lx9n.jpg"
                };
                var game4 = new Game
                {
                    Name = "Pokemon",
                    PageName = "Pokemon",
                    ImageUrl = "https://logos-world.net/wp-content/uploads/2020/05/Pokemon-Symbol.jpg"
                };
                var game5 = new Game
                {
                    Name = "Cardfight: Vanguard",
                    PageName = "CardfightVanguard",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/1/18/Vg_new_logo.png"
                };
                var game6 = new Game
                {
                    Name = "Marvel HeroClix",
                    PageName = "MarvelHeroClix",
                    ImageUrl = "https://www.nicepng.com/png/detail/306-3067793_guardians-of-the-galaxy-heroclix-now-available-age.png"
                };

                var category = new Category
                {
                    Name = "Booster Box",
                    PageName= "Category"
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

                var set1 = new Set
                {
                    Name = "Battles of Legend: Monstrous Revenge",
                    GameId= 1
                };

                var set2 = new Set
                {
                    Name = "Legacy of Destruction",
                    GameId = 1
                };

                var set3 = new Set
                {
                    Name = "Battles of Legend: Crystal Revenge",
                    GameId = 1
                };

                _context.Add(set1);
                _context.Add(set2);
                _context.Add(set3);
                _context.SaveChanges();

                var card1 = new Card
                {
                    Name = "Ukiyoe-P.U.N.K. Amazing Dragon",
                    Price = 0.63m,
                    ImageUrl = "https://images.ygoprodeck.com/images/cards/44708154.jpg",
                    StockQuantity = 21,
                    GameId = 1,
                    SetId = 1,
                    Type = 1,
                    Rarity = "Secret Rare"
                };

                var card2 = new Card
                {
                    Name = "Flowering Etoile the Melodious Magnificat",
                    Price = 1.23m,
                    ImageUrl = "https://images.ygoprodeck.com/images/cards/83793721.jpg",
                    StockQuantity = 11,
                    GameId = 1,
                    SetId = 2,
                    Type = 1,
                    Rarity = "Super Rare"
                };

                var card3 = new Card
                {
                    Name = "Blackbeard, the Plunder Patroll Captain",
                    Price = 0.63m,
                    ImageUrl = "https://images.ygoprodeck.com/images/cards/67647362.jpg",
                    StockQuantity = 7,
                    GameId = 1,
                    SetId = 3,
                    Type = 1,
                    Rarity = "Secret Rare"
                };

                var categorygame1 = new CategoryGame
                {
                    CategoryId = 1,
                    GameId = 2
                };

                _context.Add(card1);
                _context.Add(card2);
                _context.Add(card3);
                _context.Add(categorygame1);

                _context.SaveChanges();

                ViewBag.Games = _context.Games.ToList();
                ViewBag.CategoryGame = _context.CategoryGames
                    .Include(cg => cg.Game)
                    .Include(cg => cg.Category)
                    .ToList();
            }

            return View();
        }
    }
}
