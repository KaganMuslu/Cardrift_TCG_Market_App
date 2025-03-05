using System.ComponentModel.DataAnnotations.Schema;

namespace Cardrift___TCG_Market_App.Models
{
    public class Deck : Product
    {
        public List<DeckCard> DeckCards { get; set; }
    }
}
