using Catlog.Models;
using System.ComponentModel.DataAnnotations;

namespace Catlog.DTO
{
    public class ProductDto
    {
        [Key]
        public int product_Id { get; set; }
        [Required]
        public string product_title { get; set; }
        // Corresponds to ProductName
        [Required]
        public string description { get; set; }
        [Required]
        public string country_origin { get; set; }
        //public string product_image { get; set; }  // Image path
        [Required]
        public string ProductImage { get; set; } // Assuming this is how the image is stored.
        [Required]
        public int category_Id { get; set; }

        public ICollection<Product_Image> product_Image { get; set; }
        public ProductWithSkuDto ProductWithSkuDto { get; set; }

        public ICollection<ProductSku> ProductSkus { get; set; }
    }
}
