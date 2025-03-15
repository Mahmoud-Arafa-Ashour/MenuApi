using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class AddingOfferItemDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferItem_Categories_CategoryId",
                table: "OfferItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferItem_Items_ItemId",
                table: "OfferItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferItem_Offers_OfferId",
                table: "OfferItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferItem",
                table: "OfferItem");

            migrationBuilder.RenameTable(
                name: "OfferItem",
                newName: "OfferItems");

            migrationBuilder.RenameIndex(
                name: "IX_OfferItem_ItemId",
                table: "OfferItems",
                newName: "IX_OfferItems_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferItem_CategoryId",
                table: "OfferItems",
                newName: "IX_OfferItems_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferItems",
                table: "OfferItems",
                columns: new[] { "OfferId", "CategoryId", "ItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OfferItems_Categories_CategoryId",
                table: "OfferItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferItems_Items_ItemId",
                table: "OfferItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferItems_Offers_OfferId",
                table: "OfferItems",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferItems_Categories_CategoryId",
                table: "OfferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferItems_Items_ItemId",
                table: "OfferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferItems_Offers_OfferId",
                table: "OfferItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferItems",
                table: "OfferItems");

            migrationBuilder.RenameTable(
                name: "OfferItems",
                newName: "OfferItem");

            migrationBuilder.RenameIndex(
                name: "IX_OfferItems_ItemId",
                table: "OfferItem",
                newName: "IX_OfferItem_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferItems_CategoryId",
                table: "OfferItem",
                newName: "IX_OfferItem_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferItem",
                table: "OfferItem",
                columns: new[] { "OfferId", "CategoryId", "ItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OfferItem_Categories_CategoryId",
                table: "OfferItem",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferItem_Items_ItemId",
                table: "OfferItem",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferItem_Offers_OfferId",
                table: "OfferItem",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");
        }
    }
}
