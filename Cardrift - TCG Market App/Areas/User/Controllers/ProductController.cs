﻿using Cardrift___TCG_Market_App.Data;
using Microsoft.AspNetCore.Mvc;

namespace Cardrift___TCG_Market_App.Areas.User.Controllers
{
    [Area("user")]
    public class ProductController : BaseController
    {
        public ProductController(AppDbContext context) : base(context)
        {
        }

        [Route("products")]
        public IActionResult Product()
        {
            var products = _context.Products.ToList();

            return View(products);
        }

        [Route("products/{category}")]
        public IActionResult Product(string category)
        {
            var products = _context.Products.ToList();

            return View("product");
        }

        [Route("products/{category}/{type}")]
        public IActionResult Product(string category, string type)
        {
            var products = _context.Products.ToList();

            return View(products);
        }
    }
}   
