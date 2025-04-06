using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Catlog.Models;

public class Product_Image
{
    [Key]
    public int PID { get; set; }
    public string ImageName { get; set; }

    public int? ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product? product { get; set; }

    public int? sku_Id { get; set; }

    [ForeignKey("sku_Id")]
    public ProductSku productSku { get; set; }



}
