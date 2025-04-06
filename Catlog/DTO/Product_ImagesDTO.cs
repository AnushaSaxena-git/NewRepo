using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel;

namespace Catlog.DTO
    ;
public class Product_ImagesDTO
{
    [Key]
    public int PID { get; set; }
    public string ImageUrl { get; set; }

    public string ImageName { get; set; }

    //[ForeignKey("ProductId")]
    //public int ProductId { get; set; }
    //public Product product { get; set; }
    public int ProductId { get; set; }     // Product ID, so we can filter images based on product

    public int? sku_id {  get; set; }
    public ProductSkuDto ProductSku { get; set; }  // Navigation property to ProductSku

}
