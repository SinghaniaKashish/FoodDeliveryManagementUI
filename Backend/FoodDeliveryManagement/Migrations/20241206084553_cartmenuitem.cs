using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryManagement.Migrations
{
    /// <inheritdoc />
    public partial class cartmenuitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Cart_MenuItemId",
                table: "Cart",
                column: "MenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_MenuItems_MenuItemId",
                table: "Cart",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_MenuItems_MenuItemId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_MenuItemId",
                table: "Cart");
        }
    }
}
