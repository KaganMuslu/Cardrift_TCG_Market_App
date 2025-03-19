using System.ComponentModel.DataAnnotations;

namespace Cardrift___TCG_Market_App.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Quantity is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1!")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero!")]
        public decimal Price { get; set; }

        #region Navigation Properties

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        #endregion
    }
}
