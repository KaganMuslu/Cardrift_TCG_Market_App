using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardrift___TCG_Market_App.Models
{
    public class Card
    {
        public int Id { get; set; }
        [Required]
        public string Set { get; set; } = string.Empty;
        public int Type { get; set; } // Monster, Spell, Trap vb.
        [Required]
        public string Rarity { get; set; } = string.Empty;

        #region Navigation Properities

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public List<DeckCard> DeckCards { get; set; } 

        #endregion
    }
}
