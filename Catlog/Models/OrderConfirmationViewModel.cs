using Catlog.DTO;

namespace Catlog.Models
{
    public class OrderConfirmationViewModel
    {
         public List<CartItem>CartItems { get; set; }
        //public  AddressViewModel <ICollection > AddressViewModel { get; set; }
        public AddressViewModel ShippingAddress { get; set; }
        public int ProductId { get; set; }
        public int? SkuId { get; set; }  // SKU is optional
        public string SkuName { get; set; }
        public string ProductTitle { get; set; }
        public string size { get; set; }

        public string material { get; set; }
        public string color { get; set; }
        public string ImageName { get; set; }

        public List<ProductSkuDto> productSkuDtos { get; set; }
        public List<ProductDto> productDtos { get; set; }
        public List<ProductWithSkuDto> ProductWithSkuDtos { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
