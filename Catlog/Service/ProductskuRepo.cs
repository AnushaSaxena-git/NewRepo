using AutoMapper;
using Catlog.DTO;
using Catlog.Models;
using Catlog.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Catlog.Service
{
    public class ProductskuRepo : IProductSkuRepository
    {
        private readonly CatlogDBcontext _context;
        private readonly IMapper _mapper;
        private readonly IProduct_ImageRepository _imageRepository;

        // Constructor without dependency on IUnitOfWork, but on the needed repositories
        public ProductskuRepo(CatlogDBcontext context, IMapper mapper, IProduct_ImageRepository imageRepository)
        {
            _context = context;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        // Get a single Product SKU by Id
        public async Task<ProductSkuDto> GetProductSkuByIdAsync(int skuId)
        {
            var productSku = await _context.ProductSkus
                .FirstOrDefaultAsync(ps => ps.sku_Id == skuId);

            if (productSku == null)
            {
                return null;
            }

            return _mapper.Map<ProductSkuDto>(productSku);
        }
        public async Task<List<ProductSkuDto>> GetProductSkuByIdAsyncall(int skuId)
        {
            var productSku =  _context.ProductSkus
                .Where(ps => ps.sku_Id == skuId);

            if (productSku == null)
            {
                return null;
            }

            return _mapper.Map<List<ProductSkuDto>>(productSku);
        }

        public async Task<List<Product_Image>> GetProductImagesBySkuId(int skuId)
        {
            return await _context.Product_Images
                .Where(img => img.sku_Id == skuId)
                .ToListAsync();
        }
        // Get Product Skus by name using stored procedure
        public async Task<IEnumerable<ProductSkuDto>> GetProductSkuAsync(string name)
        {
            var parameters = new[]
            {
                new SqlParameter("@productname", SqlDbType.NVarChar) { Value = (object)name ?? DBNull.Value }
            };

            var productSkus = await _context.ProductSkus
                .FromSqlRaw("EXEC dbo.getproductbyname @productname", parameters)
                .ToListAsync();

            return _mapper.Map<List<ProductSkuDto>>(productSkus);
        }

        // Add Product SKUs in a loop, handling multiple sizes, materials, etc.
        public async Task<IEnumerable<ProductSkuDto>> AddProductSkuAsync(
            string[] sizes,
            string[] materials,
            int[] prices,
            int[] stockQuantities,
            int productId,
            string[] skuNames,
            string[] colors,
            string[] skuImages)
        {
            var productSkus = new List<ProductSku>();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Begin a transaction
                //await _context.Database.BeginTransactionAsync();

                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                for (int i = 0; i < sizes.Length; i++)
                {
                    for (int j = 0; j < materials.Length; j++)
                    {
                        for (int k = 0; k < prices.Length; k++)
                        {
                            for (int l = 0; l < stockQuantities.Length; l++)
                            {
                                for (int m = 0; m < colors.Length; m++)
                                {
                                        var productSku = new ProductSku
                                    {
                                        size = sizes[i],
                                        material = materials[j],
                                        Price = prices[k],
                                        stockQuantity = stockQuantities[l],
                                        product_Id = productId,
                                            sku_Name = skuNames.Length > 0 ? skuNames[i % skuNames.Length] : "Default Name",
                                        color = colors[m]
                                    };

                                    // Add SKU to the context but don't save immediately
                                    _context.ProductSkus.Add(productSku);
                                    product.ProductSkus.Add(productSku);
                                    await _context.SaveChangesAsync();

                                    productSkus.Add(productSku);
                                    await _context.SaveChangesAsync();
                                    await transaction.CommitAsync();

                                    // Commit the transaction 
                                    // If SKU images are provided, associate them with the SKU
                                    using var transaction2 = _context.Database.BeginTransaction();
                                    if (skuImages != null && skuImages.Length > 0)
                                    {
                                        foreach (var skuImageName in skuImages)
                                        {
                                            if (string.IsNullOrWhiteSpace(skuImageName))
                                            {
                                                Console.WriteLine($"Warning: Invalid SKU Image Name - {skuImageName}");
                                                continue;
                                            }

                                            // Insert SKU image in a separate repository call
                                            await _imageRepository.InsertProductSkuImageAsync(productSku.sku_Id, skuImageName);
                                        }
                                    }
                                    await _context.SaveChangesAsync();

                                    // Commit the transaction 
                                    await transaction2.CommitAsync();
                                }
                            }
                        }
                    }
                }



                return _mapper.Map<IEnumerable<ProductSkuDto>>(productSkus);
            }
            catch (Exception ex)
            {
                // Rollback 
                await transaction.RollbackAsync();
                Console.WriteLine($"Error adding product SKU: {ex.Message}");
                throw new Exception("Error adding product SKU", ex);
            }
        }

        // Get all Product Skuss
        public async Task<List<ProductSkuDto>> GetProductAsync()
        {
            var product = await _context.ProductSkus.ToListAsync();
            return _mapper.Map<List<ProductSkuDto>>(product);
        }

        public async Task<List<ProductSkuDto>> GetProductSkuById(int id)
        {
            var skus = await _context.ProductSkus

                                 .Where(sku => sku.product_Id == id)
                                 .Select(sku => new ProductSkuDto
                                 {
                                     sku_Id = sku.sku_Id,
                                     skuname = sku.sku_Name,
                                     color = sku.color,
                                     size = sku.size,
                                     material = sku.material,
                                     Price = sku.Price,
                                     stockQuantity = sku.stockQuantity,
                                     // Map other fields...
                                 })
                                 .ToListAsync();

            return skus;

        }

       




        // Retrieve all Product Skus 
        public async Task<List<ProductSkuDto>> getproduct()
        {
            try
            {
                // Retrieve all ProductSkus from the database
                var productSkus = await _context.ProductSkus.ToListAsync();

                // Map the retrieved ProductSkus to ProductSkuDto using AutoMapper
                return _mapper.Map<List<ProductSkuDto>>(productSkus);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving product SKUs", ex);
            }
        }

        // Save changes to the database
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<ProductSkuDto> GetProductById(int id)


          {
            // Retrieve the ProductSku from the database
            var products = await _context.Products.FindAsync(id);

            // If product SKU is found, map it to ProductSkuDto
            if (products == null)
            {
                return null;
            }

            // Use AutoMapper to map the ProductSku entity to ProductSkuDto
            return _mapper.Map<ProductSkuDto>(products);
        }
        public async Task UpdateProductSkuAsync(int productId, string size, string material, int price, int stockQuantity, string skuName, string color)
        {
            var sku = await _context.ProductSkus.FirstOrDefaultAsync(s => s.product_Id == productId && s.sku_Name == skuName);
            if (sku != null)
            {
                sku.size = size;
                sku.material = material;
                sku.Price = price;
                sku.stockQuantity = stockQuantity;
                sku.color = color;

                _context.ProductSkus.Update(sku);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteProductSkuAsync(int skuId)
        {
            var sku = await _context.ProductSkus.FindAsync(skuId);
            if (sku != null)
            {
                _context.ProductSkus.Remove(sku);
                await _context.SaveChangesAsync();
            }
        }

    }
}
