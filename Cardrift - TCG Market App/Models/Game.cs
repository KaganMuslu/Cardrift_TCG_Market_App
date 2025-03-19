using System.ComponentModel.DataAnnotations;

namespace Cardrift___TCG_Market_App.Models
{
    public class Game
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Game name is required!")]
        [StringLength(100, ErrorMessage = "Game name must be at most 100 characters!")]
        public string Name { get; set; }

        public string? ImageUrl { get; set; }

        #region Navigation Properties
        public List<Product>? Products { get; set; } = new();
        #endregion
    }
}
