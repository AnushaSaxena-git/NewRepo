using Catlog.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Catlog.DTO
{
    public class ParentCategory
    {
        [Key]
        public int parentCategory_Id { get; set; }


        public int parentCategoryname { get; set; }
        [ForeignKey("Category")]

        public int category_Id { get; set; }
        public ICollection<Category> Categories { get; set; }
        //public ICollection<Product> Products{ get; set; }


    }
}
