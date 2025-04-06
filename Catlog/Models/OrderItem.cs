using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catlog.Models
{
    public class OrderItem
    {
        [Key]
        public int orderItemId {  get; set; } 

        public  int productId { get; set; }

        public int quantity { get; set; }
        public decimal price { get; set; }
        public string size {  get; set; }
        public int orderId { get; set; }
        [ForeignKey(nameof(orderId))]
        public Order order { get; set; }   
    }
}
