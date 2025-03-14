﻿using System.ComponentModel.DataAnnotations;

namespace Cardrift___TCG_Market_App.Models
{
    public class Game
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? ImageUrl { get; set; }

        #region Navigation Properities

        public List<Product>? Products { get; set; }

        #endregion
    }
}
