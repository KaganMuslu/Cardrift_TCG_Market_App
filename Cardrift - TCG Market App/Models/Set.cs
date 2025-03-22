using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardrift___TCG_Market_App.Models
{
    public class Set
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Set name is required!")]
        [StringLength(100, ErrorMessage = "Set name must be at most 100 characters!")]
        public string Name { get; set; }

        #region Navigation Properties

        [ForeignKey("Game")]
        public int GameId { get; set; }

        public List<Card>? Cards { get; set; } = new();

        #endregion
    }
}
