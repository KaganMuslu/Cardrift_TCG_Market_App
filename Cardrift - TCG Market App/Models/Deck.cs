using System.ComponentModel.DataAnnotations.Schema;

namespace Cardrift___TCG_Market_App.Models
{
    public class Deck
    {
        public int Id { get; set; }

        #region Navigation Properities
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public List<DeckCard> DeckCards { get; set; }
        #endregion
    }
}
