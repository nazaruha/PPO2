using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPO2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ManufacturerNamemakeunique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_Name",
                table: "Manufacturers",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_Name",
                table: "Manufacturers");
        }
    }
}
