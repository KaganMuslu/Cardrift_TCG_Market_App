namespace Cardrift___TCG_Market_App.Models
{
    public class DeckCard
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int DeckId { get; set; }
        public Deck? Deck { get; set; }

        public int CardId { get; set; }
        public Card? Card { get; set; }
    }
}
