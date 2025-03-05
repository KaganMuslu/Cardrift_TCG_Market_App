using System.ComponentModel.DataAnnotations.Schema;

namespace Cardrift___TCG_Market_App.Models
{
    public class Card : Product
    {
        public string Set { get; set; }
        public int Type { get; set; } // Monster, Spell, Trap vb.
        public string Rarity { get; set; }

        public List<DeckCard> DeckCards { get; set; }
    }
}
