using Catlog.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Catlog.DTO
{
    public class CategoryDto
    {


        [Key]
        public int category_Id { get; set; }

        public string category_Name { get; set; }

        //[ForeignKey("ParentCategory")]

        //public int parentcategory_Id { get; set; }

        [ForeignKey("Product")]
        ////////////////////////
        public int product_Id { get; set; }


        public ParentCategory ParentCategory { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
