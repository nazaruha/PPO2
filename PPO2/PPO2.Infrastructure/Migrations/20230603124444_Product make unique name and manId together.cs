using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPO2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductmakeuniquenameandmanIdtogether : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_Name_ManufacturerId",
                table: "Products",
                columns: new[] { "Name", "ManufacturerId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Name_ManufacturerId",
                table: "Products");
        }
    }
}
