using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class Fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "01969f7b-622a-7fd8-a041-57743843d5a0",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECy+rhgyfgbJX9K1bcES+HAtPQfB1Z2fNq29LRXPIa1KnYZjAFz3gZ7p1kT0WwZm8A==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "01969f7b-622a-7fd8-a041-57743843d5a0",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELykmw0CC06isUNHr9fkQJV7QedXg4wO+vmIXSi1P9+YJm3PBkFEnL2d5rHJOgsKGQ==");
        }
    }
}
