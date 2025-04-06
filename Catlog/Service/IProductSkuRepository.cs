using System.Collections.Generic;
using System.Threading.Tasks;
using Catlog.DTO;
using Catlog.Models;
using Microsoft.EntityFrameworkCore;

namespace Catlog.Service
{
    public interface IProductSkuRepository
    {
        // Get a ProductSku by its ID
        Task<ProductSkuDto> GetProductSkuByIdAsync(int skuId);

        Task<List<ProductSkuDto>> GetProductAsync();

        // Get Product Skus based on product name
        Task<IEnumerable<ProductSkuDto>> GetProductSkuAsync(string name);

        Task<List<ProductSkuDto>> getproduct();

        Task<List<ProductSkuDto>> GetProductSkuById(int id);

         Task<ProductSkuDto> GetProductById(int id);
         Task<List<Product_Image>> GetProductImagesBySkuId(int skuId);
       Task<List<ProductSkuDto>> GetProductSkuByIdAsyncall(int skuId);




        Task<IEnumerable<ProductSkuDto>> AddProductSkuAsync(
            string[] sizes,
            string[] materials,
            int[] prices,
            int[] stockQuantities,
            int productId,
            string[] skuNames,
            string[] colors,
            string[] skuImages); // Parameter for SKU images

        // Save changes to the database
        Task SaveChangesAsync();
             Task UpdateProductSkuAsync(int productId, string size, string material, int price, int stockQuantity, string skuName, string color);
        Task DeleteProductSkuAsync(int skuId);
    }
}
