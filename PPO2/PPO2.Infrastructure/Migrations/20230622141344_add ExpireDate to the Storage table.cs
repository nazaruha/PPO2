﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPO2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addExpireDatetotheStoragetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "Products");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "Storage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "Storage");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
