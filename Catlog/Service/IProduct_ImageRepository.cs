using Catlog.DTO;
using Catlog.Models;
using Microsoft.CodeAnalysis;

namespace Catlog.Service
{
    public interface IProduct_ImageRepository
    {

         //Task<List<Product_ImagesDTO>> AddProductImageAsync(Product_ImagesDTO productImageDTO);

        Task<Product_ImagesDTO> InsertProductImageAsync(int PID, string ImageName);
        Task InsertProductSkuImageAsync(int productId, string skuImageName);

         dynamic GetProduct_Images();
        Task InsertProductImageAsync(Product_Image productImage);

        Task<List<Product_Image>> GetImagesForSkuAsync(int skuId);


        Task<List<Product_Image>> GetProductImagesByProductId(int id);
        Task DeleteProductImageAsync( int productId);


    }
}
