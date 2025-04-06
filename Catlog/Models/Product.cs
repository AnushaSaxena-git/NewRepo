using Catlog.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    public int product_Id { get; set; }

    [Required]
    [Column("product_name", TypeName = "varchar(20)")]
    public string product_title { get; set; }
    public string description { get; set; }

    public string country_origin { get; set; }


    //public string color { get; set; }


    //public string Material { get; set; }

    //public int size { get; set; }
    //public int stockQuantity { get; set; }
    //[Required]
    //[Column("product_image", TypeName = "varchar(255)")]
  //public string? product_image { get; set; }  
    //[Required]
    //[Column("Price")]
    //[DataType(DataType.Currency)]
    //public int price { get; set; }

    public int category_Id { get; set; }
    [ForeignKey("category_Id")]
    public Category? category { get; set; }

    // ParentCategory is now nullable
    //[ForeignKey("ParentCategory")]
    //public int? parentcategory_Id { get; set; } 
    //public ParentCategory ParentCategory { get; set; }

    //[ForeignKey("ProductSku")]
    //public int sku_Id { get; set; }

    //[ForeignKey("Orders")]
    //public int order_Id { get; set; }
    //  public string imageapth {  get; set; }  

    // public ICollection<Orders> Orders { get; set; }
    public int? shipmentModel_Id { get; set; }
    [ForeignKey("shipmentModel_Id")]
    public ICollection<ShipmentModel> Shipments { get; set; }

    public ICollection<Product_Image> product_Image { get; set; }

    public ICollection<ProductSku> ProductSkus { get; set; }
}
