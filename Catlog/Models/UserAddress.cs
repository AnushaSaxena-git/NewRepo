using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catlog.Models
{
    public class UserAddress
    {
        [Key]
        public int addressId { get; set; }
        public string city {  get; set; }   
        public  int postalCode { get; set; }

        public string addressLine1 {  get; set; } 
        
        public string addressLine2 { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

    }
}
