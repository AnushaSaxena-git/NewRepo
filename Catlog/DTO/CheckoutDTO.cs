using Catlog.Models;

namespace Catlog.DTO
{
    public class CheckoutDTO
    {
        public UserDetailsDTO? User { get; set; }
        public UserAddressDTO? UserAddress { get; set; }
        public List<CartItem> CartItem { get; set; }

        public CheckoutDTO()
        {
            User = new();
            UserAddress = new();
            CartItem = new();
        }
    }

    public class UserDetailsDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class UserAddressDTO
    {
        public int addressId { get; set; }
        public string city { get; set; }
        public int postalCode { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public int UserId { get; set; }
    }
    public class OrderItemDTO
    {

    }
}