using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Catlog.DTO;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Catlog.Models.EnumExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Catlog.Models
{
    public class CatlogDBcontext : DbContext
    {
        public CatlogDBcontext(DbContextOptions<CatlogDBcontext> options) : base(options)

        {






        }
        //public DbSet<User> Users { get; set; }

        //public DbSet<Orders> Orders { get; set; }

        public DbSet<ProductSku> ProductSkus { get; set; }


        public DbSet<Category> Categories { get; set; }
        public DbSet<ParentCategory> ParentCategories { get; set; }
        //public DbSet<Product_Images> product_Images { get; set; }
        public DbSet<ShipmentModel> ShipmentModels { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Product_Image>Product_Images { get; set; }
        public DbSet<Order> Order { get; set; }   
        public DbSet<User> User { get; set; } 
        public DbSet<OrderItem> OrderItems { get; set; }        
        public DbSet<UserAddress> UserAddress { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        {
           // modelBuilder.Entity<SelectListGroup>().HasNoKey();
        }
            modelBuilder.Entity<Product_Image>()
            .HasOne(p => p.product)
            .WithMany(p => p.product_Image)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.NoAction); // Adjust as needed
            modelBuilder.Entity<ShipmentModel>()
                 .Ignore(s => s.shipmentOptionsList)
                 .Ignore(s=>s.FedexShipmentOptionsList)
                 .Ignore(s=>s.UpsShipmentOptionsList);

            modelBuilder.Entity<Product>()
       .HasOne(p => p.category)
       .WithMany(c => c.Products)
       .HasForeignKey(p => p.category_Id)  // Ensure this is mapped correctly
       .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShipmentModel>()
        .Property(s => s.ShipmentOption)
        .HasConversion<string>(); // For string-based enums

            modelBuilder.Entity<ShipmentModel>()
                .Property(s => s.fedexShipments)
                .HasConversion<string>();

            modelBuilder.Entity<ShipmentModel>()
                .Property(s => s.UpsShipment)
                .HasConversion<string>();

            // Optional: Specify delete behavior
            //.Ignore(s => s.ExpeditedDeliveryOptions)
            //.Ignore(s => s.StandardDeliveryOptions)
            //.Ignore(s => s.InternationalExpeditedServiceOptions)
            //.Ignore(s => s.FreightOptions);

            // One-to-many relationship: Category has many Products
            //modelBuilder.Entity<Category>()
            //    .HasMany(c => c.Products)
            //    .WithOne(p => p.category)
            //    .HasForeignKey(p => p.category_Id)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<User>()
            //.HasMany(u => u.Orders)
            //.WithOne(o => o.user)
            //.HasForeignKey(o => o.user)
            //.OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<ProductSku>()
            //.HasMany(p => p.product_Id)
            //.WithMany(ps => ps.ProductSkus)
            //.HasForeignKey(ps => ps.Product)
            //.OnDelete(DeleteBehavior.NoAction);

            // modelBuilder.Entity<Product>()
            //.HasMany(p => p.ProductSkus)
            //.WithOne(ps => ps.Product)
            //.HasForeignKey(ps => ps.sku_Id)
            //.OnDelete(DeleteBehavior.NoAction);




            //modelBuilder.Entity<User>()
            //.HasMany(u => u.Orders)
            //.WithOne(o => o.user)
            //.HasForeignKey(o => o.user_Id)
            //.OnDelete(DeleteBehavior.Cascade);


            //modelBuilder.Entity<Orders>()
            //.HasMany(p => p.products)
            //.WithOne(o => o.Orders)
            //.HasForeignKey(o => o.product_Id)
            //.OnDelete(DeleteBehavior.Restrict); // Adjusted behavior for orders


            //modelBuilder.Entity<ParentCategory>()
            //.HasMany(p => p.Categories)
            //.WithOne(c => c.ParentCategory)
            //.HasForeignKey(c => c.parentcategory_Id)
            //.OnDelete(DeleteBehavior.SetNull);

            // modelBuilder.Entity<Product>()
            //    .HasOne(p=>p.ParentCategory)
            //.WithOne(c => c.category)
            //.HasForeignKey(c => c.parentcategory_Id)
            //.OnDelete(DeleteBehavior.SetNull);

        }
        //public DbSet<Catlog.DTO.ProductDto> ProductDto { get; set; } = default!;
        //public DbSet<Catlog.DTO.ProductSkuDto> ProductSkuDto { get; set; } = default!;
        //public DbSet<Catlog.DTO.ProductWithSkuDto> ProductWithSkuDto { get; set; } = default!;
        //public DbSet<Catlog.DTO.ProductWSkuDto> ProductWithSkuDto { get; set; } = default!;

    }

}

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        base.OnModelCreating(modelBuilder);

//        //// One-to-many relationship: ParentCategory has many Categories
//        modelBuilder.Entity<ParentCategory>()
//            .HasMany(p => p.Categories)
//            .WithOne(c => c.ParentCategory)
//            .HasForeignKey(c => c.parentcategory_Id)
//            .OnDelete(DeleteBehavior.SetNull);

//        // One-to-many relationship: Category has many Products
//        modelBuilder.Entity<Category>()
//            .HasMany(c => c.Products)
//            .WithOne(p => p.category)
//            .HasForeignKey(p => p.category_Id)
//            .OnDelete(DeleteBehavior.Cascade);

//        // One-to-many relationship: Product has many ProductSkus
//        modelBuilder.Entity<Product>()
//            .HasMany(p => p.ProductSkus)
//            .WithOne(ps => ps.Product)
//            .HasForeignKey(ps => ps.product_Id)
//            .OnDelete(DeleteBehavior.NoAction);


//        // One-to-many relationship: User has many Orders
//        modelBuilder.Entity<User>()
//            .HasMany(u => u.Orders)
//            .WithOne(o => o.user)
//            .HasForeignKey(o => o.user_Id)
//            .OnDelete(DeleteBehavior.Cascade);

//        // One-to-many relationship: Product has many Orders
//        modelBuilder.Entity<Product>()
//            .HasMany(p => p.Orders)
//            .WithOne(o => o.product)
//            .HasForeignKey(o => o.product_Id)
//            .OnDelete(DeleteBehavior.SetNull); // Adjusted behavior for orders

//        // Set column type and precision for Price property in ProductSku
//        modelBuilder.Entity<ProductSku>()
//            .Property(p => p.Price)
//            .HasColumnType("decimal(18,2)"); // Specify precision and scale

//        // No need to directly link ParentCategory to Orders.
//    }

//}


//}

