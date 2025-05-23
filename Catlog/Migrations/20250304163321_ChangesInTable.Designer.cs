﻿// <auto-generated />
using Catlog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Catlog.Migrations
{
    [DbContext(typeof(CatlogDBcontext))]
    [Migration("20250304163321_ChangesInTable")]
    partial class ChangesInTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Catlog.Models.Category", b =>
                {
                    b.Property<int>("category_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("category_Id"));

                    b.Property<string>("category_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("parentcategory_Id")
                        .HasColumnType("int");

                    b.HasKey("category_Id");

                    b.HasIndex("parentcategory_Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Catlog.Models.Order", b =>
                {
                    b.Property<int>("orderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("orderId"));

                    b.Property<string>("AddressLine1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressLine2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderItemId")
                        .HasColumnType("int");

                    b.Property<int>("PostalCode")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("orderId");

                    b.HasIndex("UserId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Catlog.Models.OrderItem", b =>
                {
                    b.Property<int>("orderItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("orderItemId"));

                    b.Property<int>("orderId")
                        .HasColumnType("int");

                    b.Property<int>("price")
                        .HasColumnType("int");

                    b.Property<int>("productId")
                        .HasColumnType("int");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.Property<int>("size")
                        .HasColumnType("int");

                    b.HasKey("orderItemId");

                    b.HasIndex("orderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Catlog.Models.ParentCategory", b =>
                {
                    b.Property<int>("parentCategory_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("parentCategory_Id"));

                    b.Property<string>("parentCategoryname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("parentCategory_Id");

                    b.ToTable("ParentCategories");
                });

            modelBuilder.Entity("Catlog.Models.ProductSku", b =>
                {
                    b.Property<int>("sku_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("sku_Id"));

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("material")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("product_Id")
                        .HasColumnType("int");

                    b.Property<string>("size")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sku_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("stockQuantity")
                        .HasColumnType("int");

                    b.HasKey("sku_Id");

                    b.HasIndex("product_Id");

                    b.ToTable("ProductSkus");
                });

            modelBuilder.Entity("Catlog.Models.Product_Image", b =>
                {
                    b.Property<int>("PID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PID"));

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("sku_Id")
                        .HasColumnType("int");

                    b.HasKey("PID");

                    b.HasIndex("ProductId");

                    b.HasIndex("sku_Id");

                    b.ToTable("Product_Images");
                });

            modelBuilder.Entity("Catlog.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Catlog.Models.UserAddress", b =>
                {
                    b.Property<int>("addressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("addressId"));

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("addressLine1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("addressLine2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("postalCode")
                        .HasColumnType("int");

                    b.HasKey("addressId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAddress");
                });

            modelBuilder.Entity("Product", b =>
                {
                    b.Property<int>("product_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("product_Id"));

                    b.Property<int>("category_Id")
                        .HasColumnType("int");

                    b.Property<string>("country_origin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("product_title")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("product_name");

                    b.Property<int?>("shipmentModel_Id")
                        .HasColumnType("int");

                    b.HasKey("product_Id");

                    b.HasIndex("category_Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ProductShipmentModel", b =>
                {
                    b.Property<int>("ShipmentsId")
                        .HasColumnType("int");

                    b.Property<int>("shipmentModel_Id")
                        .HasColumnType("int");

                    b.HasKey("ShipmentsId", "shipmentModel_Id");

                    b.HasIndex("shipmentModel_Id");

                    b.ToTable("ProductShipmentModel");
                });

            modelBuilder.Entity("ShipmentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SelectedShipmentOption")
                        .HasColumnType("int");

                    b.Property<int?>("SelectedupsShipmentOptions")
                        .HasColumnType("int");

                    b.Property<string>("ShipmentOption")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpsShipment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fedexShipments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("product_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ShipmentModels");
                });

            modelBuilder.Entity("Catlog.Models.Category", b =>
                {
                    b.HasOne("Catlog.Models.ParentCategory", "ParentCategory")
                        .WithMany("Categories")
                        .HasForeignKey("parentcategory_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("Catlog.Models.Order", b =>
                {
                    b.HasOne("Catlog.Models.User", "User")
                        .WithMany("orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Catlog.Models.OrderItem", b =>
                {
                    b.HasOne("Catlog.Models.Order", "order")
                        .WithMany("OrderItem")
                        .HasForeignKey("orderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("order");
                });

            modelBuilder.Entity("Catlog.Models.ProductSku", b =>
                {
                    b.HasOne("Product", "Product")
                        .WithMany("ProductSkus")
                        .HasForeignKey("product_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Catlog.Models.Product_Image", b =>
                {
                    b.HasOne("Product", "product")
                        .WithMany("product_Image")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Catlog.Models.ProductSku", "productSku")
                        .WithMany("product_Images")
                        .HasForeignKey("sku_Id");

                    b.Navigation("product");

                    b.Navigation("productSku");
                });

            modelBuilder.Entity("Catlog.Models.UserAddress", b =>
                {
                    b.HasOne("Catlog.Models.User", "User")
                        .WithMany("UserAddresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Product", b =>
                {
                    b.HasOne("Catlog.Models.Category", "category")
                        .WithMany("Products")
                        .HasForeignKey("category_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("category");
                });

            modelBuilder.Entity("ProductShipmentModel", b =>
                {
                    b.HasOne("ShipmentModel", null)
                        .WithMany()
                        .HasForeignKey("ShipmentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Product", null)
                        .WithMany()
                        .HasForeignKey("shipmentModel_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Catlog.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Catlog.Models.Order", b =>
                {
                    b.Navigation("OrderItem");
                });

            modelBuilder.Entity("Catlog.Models.ParentCategory", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("Catlog.Models.ProductSku", b =>
                {
                    b.Navigation("product_Images");
                });

            modelBuilder.Entity("Catlog.Models.User", b =>
                {
                    b.Navigation("UserAddresses");

                    b.Navigation("orders");
                });

            modelBuilder.Entity("Product", b =>
                {
                    b.Navigation("ProductSkus");

                    b.Navigation("product_Image");
                });
#pragma warning restore 612, 618
        }
    }
}
