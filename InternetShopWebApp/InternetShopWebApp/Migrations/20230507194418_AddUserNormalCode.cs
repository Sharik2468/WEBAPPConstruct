using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternetShopWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNormalCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NormalCode",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "NormalCode",
                table: "AspNetUsers");
        }
    }
}
