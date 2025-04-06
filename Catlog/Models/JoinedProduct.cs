using Catlog.DTO;
using System.Linq;


namespace Catlog.Models
{
    public class JoinedProduct
    {
        public ProductSkuDto Sku { get; set; }
        public Product Product { get; set; }
    }
}   
