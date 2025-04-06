using Catlog.DTO;

namespace Catlog.Models
{
    public class CartItem
    {



        public int ProductId { get; set; }
        public int? SkuId { get; set; }  // SKU is optional
        public string SkuName { get; set; }
        public string ProductTitle { get; set; }
        public string size { get; set; }

        public string material { get; set; }
        public string color { get; set; }
        public string ImageName { get; set; }

        public List <ProductSkuDto>productSkuDtos{get; set;}
        public List<ProductDto> productDtos { get; set; }
        public List<ProductWithSkuDto> ProductWithSkuDtos { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
         public List<AddressViewModel>? addresses { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
