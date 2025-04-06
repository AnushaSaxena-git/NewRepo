using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catlog.Models
{
    public class Category
    {
        [Key]
        public int category_Id { get; set; }

        public string category_Name { get; set; }

        public int parentcategory_Id { get; set; }  

        [ForeignKey("parentcategory_Id")]  
        public ParentCategory ParentCategory { get; set; }  

        public ICollection<Product> Products { get; set; }
    }

}
