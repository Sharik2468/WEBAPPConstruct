using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternetShopWebApp.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cathegory",
                columns: table => new
                {
                    Cathegory_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cathegory_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parent_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cathegory", x => x.Cathegory_ID);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Order_Item_Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_Sum = table.Column<int>(type: "int", nullable: true),
                    Amount_Order_Item = table.Column<int>(type: "int", nullable: true),
                    Product_Code = table.Column<int>(type: "int", nullable: true),
                    Order_Code = table.Column<int>(type: "int", nullable: true),
                    Status_Order_Item_Table_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Order_Item_Code);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Product_Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderItem_Code = table.Column<int>(type: "int", nullable: false),
                    NumberInStock = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    DateOfManufacture = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchasePrice = table.Column<int>(type: "int", nullable: false),
                    MarketPrice = table.Column<int>(type: "int", nullable: false),
                    BestBeforeDate = table.Column<float>(type: "real", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Product_Code);
                    table.ForeignKey(
                        name: "FK_Product_OrderItem_OrderItem_Code",
                        column: x => x.OrderItem_Code,
                        principalTable: "OrderItem",
                        principalColumn: "Order_Item_Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_OrderItem_Code",
                table: "Product",
                column: "OrderItem_Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cathegory");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "OrderItem");
        }
    }
}
