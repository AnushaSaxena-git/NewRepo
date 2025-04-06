using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catlog.Models
{
    public class Order
    {
        [Key]
        public int orderId { get; set; } 
      public string City { get; set; }
        public int? PostalCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public string CardNumber { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ExpDate { get; set; }
        public int CVV { get; set; }

        public decimal TotalAmount { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public int OrderItemId { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; }

    }
}
