using System.ComponentModel.DataAnnotations;

namespace Cardrift___TCG_Market_App.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required!")]
        [StringLength(100, ErrorMessage = "Category name must be at most 100 characters!")]
        public string Name { get; set; }

        #region Navigation Properties
        public List<Product>? Products { get; set; } = new();
        #endregion
    }
}
