using Catlog.DTO;
using Catlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catlog.Service
{
    public interface IProductRepository
    {
          Task<List<Product>> getproduct();
        //Task<IActionResult> insertproduct();

        Task<ProductDto> InsertProductAsync(string productName, int categoryId, string countryOrigin, string description);

        Task<List<Category>> getcategories();
         Task<List<Product>> getProductsWithSkus();


        Task SaveChangesAsync();
        Task<Product> GetProductById(int id);
        //Task<ProductSkuDto> GetSkuByProductId(int id);

         Task<ProductDto> GetProductWithSkuAndImagesAsync(int productId);
        Task UpdateProductAsync(int productId, string productName, int categoryId, string countryOrigin, string description);
        Task DeleteProductAsync(int productId);



    }
}
