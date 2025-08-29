using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuthorization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDeleted", "Name", "NormalizedName", "isDefault" },
                values: new object[,]
                {
                    { "01969f7b-622a-7fd8-a041-5777b032bfa9", "01969f7b-622a-7fd8-a041-5778339daaab", false, "Admin", "ADMIN", false },
                    { "01969f7b-622a-7fd8-a041-5779e9a0ce8b", "01969f7b-622a-7fd8-a041-577a817dd965", false, "Owner", "OWNER", true }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Adress", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ResturnatName", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "01969f7b-622a-7fd8-a041-57743843d5a0", 0, "Digital Menu Street", "01969f7b-622a-7fd8-a041-5776080ebb3e", "Admin@Gmail.com", true, false, null, "Digital Menu", "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAENa2BoTsGfnVol62wpDi6wA8yML6MQFkpUQdYCwTaNrYcSV2kgmjDqFEhLGhMbNMug==", "01234567890", false, "Digital Menu Restaurant", "01969f7b622a7fd8a04157757267ddaf", false, "Admin@Gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permissions", "info", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 2, "Permissions", "UpdateInfo", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 3, "Permissions", "ChangePassword", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 4, "Permissions", "Category:ReadAll", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 5, "Permissions", "Category:GetByid", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 6, "Permissions", "Category:Add", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 7, "Permissions", "Category:Update", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 8, "Permissions", "Category:Delete", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 9, "Permissions", "Discount:Add", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 10, "Permissions", "Discount:Update", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 11, "Permissions", "Discount:Delete", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 12, "Permissions", "Item:ReadAll", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 13, "Permissions", "Item:GetByid", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 14, "Permissions", "Item:Add", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 15, "Permissions", "Item:Update", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 16, "Permissions", "Item:Delete", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 17, "Permissions", "OfferItem:ReadAll", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 18, "Permissions", "OfferItem:Add", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 19, "Permissions", "OfferItem:Update", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 20, "Permissions", "Offer:ReadAll", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 21, "Permissions", "Offer:GetByid", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 22, "Permissions", "Offer:Add", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 23, "Permissions", "Offer:Update", "01969f7b-622a-7fd8-a041-5777b032bfa9" },
                    { 24, "Permissions", "Offer:Delete", "01969f7b-622a-7fd8-a041-5777b032bfa9" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "01969f7b-622a-7fd8-a041-5777b032bfa9", "01969f7b-622a-7fd8-a041-57743843d5a0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01969f7b-622a-7fd8-a041-5779e9a0ce8b");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "01969f7b-622a-7fd8-a041-5777b032bfa9", "01969f7b-622a-7fd8-a041-57743843d5a0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01969f7b-622a-7fd8-a041-5777b032bfa9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "01969f7b-622a-7fd8-a041-57743843d5a0");
        }
    }
}
