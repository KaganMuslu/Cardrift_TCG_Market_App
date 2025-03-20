using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardrift___TCG_Market_App.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required!")]
        [StringLength(100, ErrorMessage = "Product name must be at most 100 characters!")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero!")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; } = "https://cdn.spruceindustries.com/images/no_image_available.png";

        [Required(ErrorMessage = "Stock quantity is required!")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative!")]
        public int StockQuantity { get; set; }

        #region Navigation Properties

        [ForeignKey("Game")]
        public int? GameId { get; set; }

        [ForeignKey("Category")]
        [Required(ErrorMessage = "Category is required!")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<Deck>? Decks { get; set; } = new();
        public List<Game>? Games { get; set; } = new();

        #endregion
    }
}
