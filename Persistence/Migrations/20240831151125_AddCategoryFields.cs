using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("47b31ca8-8816-42ee-8843-10d10a0f1555"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6f1a3b79-27a0-48d4-b9c7-62099fde3ee0"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b8eb0f8e-a4c4-4841-9756-da7161317006"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("f1ce6039-cdf1-4814-8f56-db9679192fcc"));

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Categories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCustom",
                table: "Categories",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "CategoryType", "IsCustom" },
                values: new object[,]
                {
                    { new Guid("83ea6329-9d77-46bc-92c9-d81a43755fd4"), "Entertainment", 3, false },
                    { new Guid("8c282001-c121-47d1-b87e-024f98440329"), "Transport", 1, false },
                    { new Guid("9e1a1c0b-1768-46dc-8940-86787a382633"), "Salary", 2, false },
                    { new Guid("cf24cfb3-1cd3-4075-8a52-eb17cfc2a9a0"), "Food", 0, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("83ea6329-9d77-46bc-92c9-d81a43755fd4"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8c282001-c121-47d1-b87e-024f98440329"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9e1a1c0b-1768-46dc-8940-86787a382633"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("cf24cfb3-1cd3-4075-8a52-eb17cfc2a9a0"));

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsCustom",
                table: "Categories");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryType" },
                values: new object[,]
                {
                    { new Guid("47b31ca8-8816-42ee-8843-10d10a0f1555"), 3 },
                    { new Guid("6f1a3b79-27a0-48d4-b9c7-62099fde3ee0"), 0 },
                    { new Guid("b8eb0f8e-a4c4-4841-9756-da7161317006"), 1 },
                    { new Guid("f1ce6039-cdf1-4814-8f56-db9679192fcc"), 2 }
                });
        }
    }
}
