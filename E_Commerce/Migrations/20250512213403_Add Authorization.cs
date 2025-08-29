using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "01969f7b-622a-7fd8-a041-57743843d5a0",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELd6eWK+Tq8bR/1q/bJKyp06rhRTc9YdpuPxQeMVmKi2zrkujmds7T3r06T+cOBH2w==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "01969f7b-622a-7fd8-a041-57743843d5a0",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENa2BoTsGfnVol62wpDi6wA8yML6MQFkpUQdYCwTaNrYcSV2kgmjDqFEhLGhMbNMug==");
        }
    }
}
