using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardrift___TCG_Market_App.Models
{
    public class Card
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Set is required!")]
        public string Set { get; set; } = string.Empty;

        [Required(ErrorMessage = "Card type is required!")]
        public int Type { get; set; } // Monster, Spell, Trap vb.

        [Required(ErrorMessage = "Rarity is required!")]
        public string Rarity { get; set; } = string.Empty;

        #region Navigation Properties

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public List<DeckCard> DeckCards { get; set; } = new();

        #endregion
    }
}
