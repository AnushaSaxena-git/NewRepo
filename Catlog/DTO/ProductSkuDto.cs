using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Catlog.DTO
{
    public class ProductSkuDto
    {
        [Key]
        public int sku_Id { get; set; }
        public string skuname { get; set; }
        public string size { get; set; }
        public List<string> Sizes { get; set; }  

        public string material { get; set; }
        public string color { get; set; }

        public int Price { get; set; }

        public string ProductImage { get; set; } 

        public int stockQuantity { get; set; }
        [DisplayName("Product Name")]
        [Required]
        public string  ProductName{get; set;}

        public List<Product_ImagesDTO> Images { get; set; }

        //[ForeignKey("Product")]
        public int product_Id { get; set; }
        //public Product Product { get; set; } 
    }
}
