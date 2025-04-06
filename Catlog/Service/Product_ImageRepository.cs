using AutoMapper;
using Catlog.DTO;
using Catlog.Models;
using Catlog.Service;
using Microsoft.EntityFrameworkCore;

namespace Catlog.Repositories
{
    public class Product_ImageRepository : IProduct_ImageRepository
    {
        private readonly IMapper _mapper;
        private readonly CatlogDBcontext _context;

        public Product_ImageRepository(CatlogDBcontext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Insert product image 
        public async Task InsertProductImageAsync(Product_Image productImage)
        {
            if (productImage != null)
            {
                await _context.Product_Images.AddAsync(productImage);
                await _context.SaveChangesAsync();
            }
        }

        // Insert SKU image 
        public async Task InsertProductSkuImageAsync(int skuId, string skuImageName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(skuImageName))
                {
                    var productSku = await _context.ProductSkus.FindAsync(skuId);
                    if (productSku == null)
                    {
                        throw new Exception("Product SKU not found.");
                    }

                    var productImage = new Product_Image
                    {
                        sku_Id = skuId,  // Associate the image with the SKU
                        ProductId = productSku.product_Id,  // Associate the image with the product
                        ImageName = skuImageName
                    };

                    await _context.Product_Images.AddAsync(productImage);
                    // yahan pe product image  ko id assign hui 
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task<List<Product_Image>> GetProductImagesByProductId(int id)
        
        {
            return await _context.Product_Images.Where(pi => pi.ProductId == id).ToListAsync();
        }

        // Insert product image and return mapped DTO
        public async Task<Product_ImagesDTO> InsertProductImageAsync(int PID, string ImageName)
        {
            var productImage = new Product_Image
            {
                ProductId = PID,
                ImageName = ImageName,
            };

            await _context.Product_Images.AddAsync(productImage);
            await _context.SaveChangesAsync();

            return _mapper.Map<Product_ImagesDTO>(productImage);
        }

        // Get all product images not in use currently 
        public dynamic GetProduct_Images()
        {
            try
            {
                var productImages = _context.Product_Images.FromSqlRaw("exec getImages");
                return productImages;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<List<Product_Image>> GetImagesForSkuAsync(int skuId)
        {
            try
            {
                var skuImages = await _context.Product_Images
                                               .Where(img => img.sku_Id == skuId)  // Filtering by SKU ID
                                               .ToListAsync();
                return skuImages;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task DeleteProductImageAsync(int productId)
        {
            try
            {
                // Find the product image by ProductId (if it exists)
                var productImage = await _context.Product_Images
                    .Where(p => p.ProductId == productId)
                    .FirstOrDefaultAsync();

                if (productImage != null)
                {
                    // Delete the image record from the database
                    _context.Product_Images.Remove(productImage);
                    await _context.SaveChangesAsync();

                    // Optionally, delete the physical image file from the server
                    var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    if (Directory.Exists(imageDirectory))
                    {
                        var imagePath = Path.Combine(imageDirectory, productImage.ImageName);

                        // Ensure the file exists before deleting
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath);
                        }
                    }

                    Console.WriteLine($"Product image for ProductId {productId} has been deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"No product image found for ProductId {productId}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product image for ProductId {productId}: {ex.Message}");
            }
        }

    }
}
