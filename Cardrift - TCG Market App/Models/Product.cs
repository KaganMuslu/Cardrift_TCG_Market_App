using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardrift___TCG_Market_App.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; } = "https://cdn.spruceindustries.com/images/no_image_available.png";
        [Required]
        public int StockQuantity { get; set; }

        #region Navigation Properities

        [ForeignKey("Game")]
        public int? GameId { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<Card>? Cards { get; set; }
        public List<Deck>? Decks { get; set; }

        #endregion
    }
}
