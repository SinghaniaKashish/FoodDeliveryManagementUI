using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryManagement.Migrations
{
    /// <inheritdoc />
    public partial class menutable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientUsages_MenuItems_MenuItemItemId",
                table: "IngredientUsages");

            migrationBuilder.DropIndex(
                name: "IX_IngredientUsages_MenuItemItemId",
                table: "IngredientUsages");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "MenuItemItemId",
                table: "IngredientUsages");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientUsages_ItemId",
                table: "IngredientUsages",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientUsages_MenuItems_ItemId",
                table: "IngredientUsages",
                column: "ItemId",
                principalTable: "MenuItems",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientUsages_MenuItems_ItemId",
                table: "IngredientUsages");

            migrationBuilder.DropIndex(
                name: "IX_IngredientUsages_ItemId",
                table: "IngredientUsages");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MenuItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MenuItemItemId",
                table: "IngredientUsages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IngredientUsages_MenuItemItemId",
                table: "IngredientUsages",
                column: "MenuItemItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientUsages_MenuItems_MenuItemItemId",
                table: "IngredientUsages",
                column: "MenuItemItemId",
                principalTable: "MenuItems",
                principalColumn: "ItemId");
        }
    }
}
