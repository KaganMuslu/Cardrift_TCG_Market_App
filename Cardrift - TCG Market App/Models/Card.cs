using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardrift___TCG_Market_App.Models
{
    public class Card
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Card name is required!")]
        [StringLength(100, ErrorMessage = "Card name must be at most 100 characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Card type is required!")]
        public int Type { get; set; } // Monster, Spell, Trap vb.

        [Required(ErrorMessage = "Rarity is required!")]
        public string Rarity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero!")]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; } = "https://cdn.spruceindustries.com/images/no_image_available.png";

        [Required(ErrorMessage = "Stock quantity is required!")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative!")]
        public int StockQuantity { get; set; }

        #region Navigation Properties

        [ForeignKey("Game")]
        public int GameId { get; set; }
        public Game Game { get; set; }

        [ForeignKey("Set")]
        public int SetId { get; set; }
        public Set Set { get; set; }

        public List<DeckCard> DeckCards { get; set; } = new();

        #endregion
    }
}
