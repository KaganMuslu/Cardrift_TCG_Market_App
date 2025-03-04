namespace Cardrift___TCG_Market_App.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; } // Pending, Shipped, Delivered vb.
    }
}
