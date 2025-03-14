﻿using Cardrift___TCG_Market_App.Data;
using Cardrift___TCG_Market_App.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cardrift___TCG_Market_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DataController : BaseController
    {
        public DataController(AppDbContext context) : base(context)
        {
        }

        public IActionResult Games(int page, string? game)
        {
            if (page == 0) page = 1;
            ViewBag.page = page;

            int skip = (page - 1) * 8;

            List<Game> games;
            int next;

            if (string.IsNullOrEmpty(game))
            {
                // Arama yoksa tüm oyunları getir
                games = _context.Games.Skip(skip).Take(8).ToList();

                // Bir sonraki sayfada eleman var mı kontrol et
                next = _context.Games.Skip(page * 8).Count();
            }
            else
            {
                // Arama varsa filtreli sonuçları getir
                var searchGame = _context.Games.Where(x => x.Name.Contains(game));

                games = searchGame.Skip(skip).Take(8).ToList();

                // Filtreli sonuçlar için next kontrolü
                next = searchGame.Skip(page * 8).Count();
            }

            ViewBag.next = next > 0;

            return View(games);
        }

        #region Game Section

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
