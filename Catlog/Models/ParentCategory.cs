using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catlog.Models
{
    public class ParentCategory
    {
        [Key]
        public int parentCategory_Id { get; set; }

        public string parentCategoryname { get; set; }

        // Remove this incorrect foreign key definition
        // [ForeignKey("category_Id")]

        // The correct definition is this collection:
        public ICollection<Category> Categories { get; set; } // Navigation property to the related Categories
    }
}
