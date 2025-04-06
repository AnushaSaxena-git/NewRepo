using Catlog.Models;

namespace Catlog.Service
{
    public interface ICartService
    {
        void AddToCart(CartItem cartItem);
        List<CartItem> GetCartItems();
        void RemoveFromCart(int itemId);
        public bool RemoveFromCartt(int skuId);

        void ClearCart();
        void UpdateCart(List<CartItem> items);
        //bool RemoveFromCart(int skuId); // Indicating success/failure of item removal
        decimal GetTotal(); // To get the total of items in the cart


    }
}
