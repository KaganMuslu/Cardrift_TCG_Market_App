using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardrift___TCG_Market_App.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero!")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Range(0, 2, ErrorMessage = "Invalid status value!")] // 0: Pending, 1: Shipped, 2: Delivered
        public int Status { get; set; }

        #region Navigation Properties

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();

        #endregion
    }
}
