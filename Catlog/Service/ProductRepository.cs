using AutoMapper;
using Catlog.Models;
using Catlog.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catlog.Service
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatlogDBcontext _context;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductRepository(CatlogDBcontext context, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        // Get all products 
        public async Task<List<Product>> GetProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        // Insert a product 
        public async Task<ProductDto> InsertProductAsync(string productName, int categoryId, string countryOrigin, string description)
        {
            try
            {
                if (string.IsNullOrEmpty(productName))
                    throw new ArgumentException("Product name cannot be null or empty", nameof(productName));

                if (categoryId <= 0)
                    throw new ArgumentException("Invalid category ID", nameof(categoryId));

                if (_context == null)
                    throw new InvalidOperationException("Database context is not initialized.");

                if (_mapper == null)
                    throw new InvalidOperationException("AutoMapper instance is not initialized.");

                // Begin transaction
                //_unitOfWork.CreateTransaction();

                // The product entity (without image at this point)
                var product = new Product
                {
                    product_title = productName,
                    category_Id = categoryId,
                    country_origin = countryOrigin,
                    description = description
                };

                _context.Products.Add(product);
             //   await _context.SaveChangesAsync(); // Save changes to get the product Id

                // Commit the transaction 
                _unitOfWork.Commit();

                // Return mapped DTO
                return _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Error inserting product", ex);
            }
        }
        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<List<Product>> getproduct()
        {

            var product = await _context.Products.ToListAsync();

            return product;

        }
        public async Task<List<Category>> getcategories()
        {

            var categories = await _context.Categories.ToListAsync();

            return categories;

        }
        public async Task<List<Product>> getProductsWithSkus()
        {
            return await _context.Products
                .Include(p => p.ProductSkus) // Assuming Product has a collection of Skus
                .ToListAsync();
        }

        // public async Task<ProductSkuDto> GetSkuByProductId(int id)
        //{
        //     var productsku= await _context.ProductSkus.FirstOrDefault(d=)
        //    return 

        //}



        // Get all categories for the product
        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ProductDto> GetProductWithSkuAndImagesAsync(int productId)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.ProductSkus)          
                    .Include(p => p.product_Image) 
                    .FirstOrDefaultAsync(p => p.product_Id == productId); 

                if (product == null)
                    throw new KeyNotFoundException($"Product with ID {productId} not found.");

                var productDto = _mapper.Map<ProductDto>(product);

               
                var productSkuDtos = _mapper.Map<List<ProductSkuDto>>(product.ProductSkus);

                var ProductImages = _mapper.Map<List<Product_ImagesDTO>>(product.product_Image);

                return productDto;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error retrieving product with SKU and images", ex);
            }

        }
        public async Task UpdateProductAsync(int productId, string productName, int categoryId, string countryOrigin, string description)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.product_title = productName;
                product.category_Id= categoryId;
                product.country_origin= countryOrigin;
                product.description = description;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
