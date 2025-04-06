using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catlog.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }    
        public string Name { get; set; }

        public string Email {  get; set; }  

        public string Password { get; set; }

        public string  Phone { get; set; }
        public ICollection<Order>orders { get; set; }
        public ICollection<UserAddress> UserAddresses { get; set; }


    }
}
