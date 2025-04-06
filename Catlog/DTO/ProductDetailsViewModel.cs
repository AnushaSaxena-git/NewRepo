using Catlog.Models;

namespace Catlog.DTO
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; } // This should represent the product details


        //public ProductDto Product { get; set; }
        public List<Product_Image> ProductImageGallery { get; set; } = new List<Product_Image>(); // Image gallery

        public List<ProductWithSkuDto> ProductSkus { get; set; }
        //public class ProductDetailsViewModel
        //  {
            public List<string> AvailableColors { get; set; }
            public List<string> AvailableSizes { get; set; }
            public List<string> AvailableMaterials { get; set; }
            public string ProductImageUrl { get; set; }
        public string Images { get; set; }
        //   public List<ProductWithSkuDto> ProductSkus { get; set; }

    }

    }

