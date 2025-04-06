using Catlog.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Catlog.Service
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private const string CartCookieName = "Cart"; // Name of the cookie

        // Helper method to get cart items from cookie
        private List<CartItem> GetCartFromCookie()
        {
            var cookie = _httpContextAccessor.HttpContext.Request.Cookies[CartCookieName];
            if (string.IsNullOrEmpty(cookie))
            {
                return new List<CartItem>();
            }

            return JsonConvert.DeserializeObject<List<CartItem>>(cookie);
        }

        // Helper method to save cart items to cookie
        private void SaveCartToCookie(List<CartItem> cart)
        {
            var cookieOptions = new CookieOptions
            {
                Secure = true, // Ensure cookie is sent over HTTPS
                SameSite = SameSiteMode.Strict, // Prevent cross-site request forgery
                Expires = System.DateTime.Now.AddDays(7) // Set cookie expiry
            };

            var cartJson = JsonConvert.SerializeObject(cart);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(CartCookieName, cartJson, cookieOptions);
        }

        public void AddToCart(CartItem cartItem)
        {
            var cart = GetCartFromCookie();
            var index = cart.FindIndex(x => x.SkuId == cartItem.SkuId && x.ProductId == cartItem.ProductId);
            if (index == -1)
            {
                cart.Add(cartItem);
            }
            else
            {
                cart[index].Quantity += 1;
            }

            SaveCartToCookie(cart);
        }

        public List<CartItem> GetCartItems()
        {
           return GetCartFromCookie();
        }

        public void UpdateCart(List<CartItem> items)
        {
            SaveCartToCookie(items);
        }

        public void RemoveFromCart(int itemId)
        {
            var cart = GetCartFromCookie();
            var item = cart.FirstOrDefault(i => i.SkuId == itemId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCartToCookie(cart);
            }
        }
        public bool RemoveFromCartt(int itemId)
        {
            var cart = GetCartFromCookie();
            var item = cart.FirstOrDefault(i => i.SkuId == itemId);

            if (item != null)
            {
                cart.Remove(item);
                SaveCartToCookie(cart);
                return true;  // Return true if the item was removed
            }
            return false;  // Return false if no item was found to remove
        }

        public decimal GetTotal()
        {
            // Fetch the cart items from the cookie
            var cart = GetCartFromCookie();

            // Calculate the total (sum of prices * quantity)
            return cart.Sum(item => item.Price * item.Quantity);
        }


        public void ClearCart()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(CartCookieName);
        }
    }
}
