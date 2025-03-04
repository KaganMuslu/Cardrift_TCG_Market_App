namespace Cardrift___TCG_Market_App.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Set { get; set; }
        public int Type { get; set; } // Monster, Spell, Trap vb.
        public string Rarity { get; set; }
        public decimal Price { get; set; }
    }
}
