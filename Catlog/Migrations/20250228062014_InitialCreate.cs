using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catlog.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParentCategories",
                columns: table => new
                {
                    parentCategory_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    parentCategoryname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentCategories", x => x.parentCategory_Id);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipmentOption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fedexShipments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpsShipment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_Id = table.Column<int>(type: "int", nullable: true),
                    SelectedupsShipmentOptions = table.Column<int>(type: "int", nullable: true),
                    SelectedShipmentOption = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    category_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parentcategory_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.category_Id);
                    table.ForeignKey(
                        name: "FK_Categories_ParentCategories_parentcategory_Id",
                        column: x => x.parentcategory_Id,
                        principalTable: "ParentCategories",
                        principalColumn: "parentCategory_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    product_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_name = table.Column<string>(type: "varchar(20)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    country_origin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category_Id = table.Column<int>(type: "int", nullable: false),
                    shipmentModel_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.product_Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_category_Id",
                        column: x => x.category_Id,
                        principalTable: "Categories",
                        principalColumn: "category_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductShipmentModel",
                columns: table => new
                {
                    ShipmentsId = table.Column<int>(type: "int", nullable: false),
                    shipmentModel_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductShipmentModel", x => new { x.ShipmentsId, x.shipmentModel_Id });
                    table.ForeignKey(
                        name: "FK_ProductShipmentModel_Products_shipmentModel_Id",
                        column: x => x.shipmentModel_Id,
                        principalTable: "Products",
                        principalColumn: "product_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductShipmentModel_ShipmentModels_ShipmentsId",
                        column: x => x.ShipmentsId,
                        principalTable: "ShipmentModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSkus",
                columns: table => new
                {
                    sku_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sku_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stockQuantity = table.Column<int>(type: "int", nullable: false),
                    product_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSkus", x => x.sku_Id);
                    table.ForeignKey(
                        name: "FK_ProductSkus_Products_product_Id",
                        column: x => x.product_Id,
                        principalTable: "Products",
                        principalColumn: "product_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Images",
                columns: table => new
                {
                    PID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    sku_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Images", x => x.PID);
                    table.ForeignKey(
                        name: "FK_Product_Images_ProductSkus_sku_Id",
                        column: x => x.sku_Id,
                        principalTable: "ProductSkus",
                        principalColumn: "sku_Id");
                    table.ForeignKey(
                        name: "FK_Product_Images_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "product_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_parentcategory_Id",
                table: "Categories",
                column: "parentcategory_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Images_ProductId",
                table: "Product_Images",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Images_sku_Id",
                table: "Product_Images",
                column: "sku_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_category_Id",
                table: "Products",
                column: "category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductShipmentModel_shipmentModel_Id",
                table: "ProductShipmentModel",
                column: "shipmentModel_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSkus_product_Id",
                table: "ProductSkus",
                column: "product_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product_Images");

            migrationBuilder.DropTable(
                name: "ProductShipmentModel");

            migrationBuilder.DropTable(
                name: "ProductSkus");

            migrationBuilder.DropTable(
                name: "ShipmentModels");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ParentCategories");
        }
    }
}
