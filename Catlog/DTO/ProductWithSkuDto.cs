using Catlog.DTO;
using System.ComponentModel.DataAnnotations;
namespace Catlog.DTO
{
    public class ProductWithSkuDto
    {
        [Key]
        public int SkuId { get; set; }
        [Required]
        public ProductDto Product { get; set; }
        [Required]

        public ShipmentDTO ShipmentDetails { get; set; }
        [Required]

        public List<Product_ImagesDTO> Images { get; set; }// thois is list  passed here to list the details and view details , editt etc  controller purpose
        [Required]

        public List<string> Colors { get; set; }
        [Required]

        public List<string> Sizes { get; set; }
        [Required]

        public List<string> Materials { get; set; }
        [Required]
        public List<ProductSkuDto> ProductSkus { get; set; } // This should be a list of SKUs// thois is list  passed here to list the details and view details , editt etc

        public int Price { get; set; }
        [Required]

        public string? ShipmentOption { get; set; }
            public string? FedexShipments { get; set; }
            public string? UpsShipment { get; set; }
            public string? From { get; set; }
            public string? Destination { get; set; }


        [Required]

        public int StockQuantity { get; set; }
        [Required]
        public List<ProductSkuDto> Skus { get; set; }

        public string SkuName { get; set; }
        public int? product_Id { get; set; }
        [Required]
        public string? product_title { get; set; }
        // Corresponds to ProductName
        [Required]
        public string? description { get; set; }
        [Required]
        public string? country_origin { get; set; }
        //public string product_image { get; set; }  // Image path
        [Required]
        public string? ProductImage { get; set; } // Assuming this is how the image is stored.
        [Required]
        public int? category_Id { get; set; }


    }
}
