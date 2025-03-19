using System.ComponentModel.DataAnnotations;

namespace Cardrift___TCG_Market_App.Models
{
    public class DeckCard
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Quantity is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1!")]
        public int Quantity { get; set; }

        public int DeckId { get; set; }
        public Deck? Deck { get; set; }

        public int CardId { get; set; }
        public Card? Card { get; set; }
    }
}
