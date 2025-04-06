using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catlog.Models
{
    public class ProductSku
    {
        [Key]
        public int sku_Id { get; set; } 
        public string sku_Name { get; set; } 
        public string size { get; set; }   

        public string material {  get; set; }

        public int Price { get; set; } 

        public string color {  get; set; }
        public int stockQuantity {  get; set; }
        //public List<Product_Images> product_Images { get; set; }



        public int product_Id { get; set; }
        [ForeignKey("product_Id")]
        public Product Product { get; set; } // Navigation property to the associated Product


        public ICollection<Product_Image> product_Images { get; set; }



    }


}
