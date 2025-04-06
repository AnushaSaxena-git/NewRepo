using AutoMapper;
using Catlog.Models;

namespace Catlog.DTO
{
    public class ProductProfile : Profile
    {

        public ProductProfile()
        {

            CreateMap<Product, ProductDto>();
            CreateMap<ProductSku, ProductSkuDto>();

            CreateMap<ParentCategory, ParentCategory>();

            CreateMap<Category, CategoryDto>();
            CreateMap<Product_Image, Product_ImagesDTO>();
            //CreateMap<User, userDto>();


            CreateMap<ProductSku, ProductSkuDto>();
            CreateMap<ProductWithSkuDto, CustomDTO>();

        }

    }
}