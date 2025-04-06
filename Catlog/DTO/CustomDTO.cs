using Catlog.Models;

namespace Catlog.DTO
{
    public class CustomDTO
    {
        public List<CartItem> CartItems { get; set; }
        public List<AddressViewModel> AddressViews { get; set; }
        
    }
}
