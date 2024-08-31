using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("73648b88-3006-46bb-a940-9b3866f188ee"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("974221c3-a7c7-43d8-9cbb-8cbbcee37e1c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e22ac127-8b2a-4af2-a6f2-9594a761d576"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e96b1a49-ac5f-4b9b-be2d-879c5f448d66"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cc33597e-710a-496d-8f20-91d3d7b5f896"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Transactions");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryType" },
                values: new object[,]
                {
                    { new Guid("73648b88-3006-46bb-a940-9b3866f188ee"), 1 },
                    { new Guid("974221c3-a7c7-43d8-9cbb-8cbbcee37e1c"), 0 },
                    { new Guid("e22ac127-8b2a-4af2-a6f2-9594a761d576"), 2 },
                    { new Guid("e96b1a49-ac5f-4b9b-be2d-879c5f448d66"), 3 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PasswordHash", "UpdatedAt" },
                values: new object[] { new Guid("cc33597e-710a-496d-8f20-91d3d7b5f896"), new DateTime(2024, 8, 27, 18, 47, 31, 987, DateTimeKind.Local).AddTicks(7052), "Admin", "AdminHash", null });
        }
    }
}
