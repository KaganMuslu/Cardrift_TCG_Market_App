namespace Cardrift___TCG_Market_App.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        #region Navigation Properities

        public List<Product> Products { get; set; }

        #endregion
    }
}
