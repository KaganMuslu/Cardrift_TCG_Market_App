using System.ComponentModel.DataAnnotations;

namespace Cardrift___TCG_Market_App.Models
{
    public class CategoryGame
    {
        public int Id { get; set; }

        #region Navigation Properties

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }

        #endregion
    }
}
