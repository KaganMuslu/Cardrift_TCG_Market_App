using System.ComponentModel.DataAnnotations;

namespace Cardrift___TCG_Market_App.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }

        #region Navigation Properities
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } 
        #endregion
    }

}
