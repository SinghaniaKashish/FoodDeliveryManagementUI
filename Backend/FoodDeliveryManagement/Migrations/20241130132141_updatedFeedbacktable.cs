using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryManagement.Migrations
{
    /// <inheritdoc />
    public partial class updatedFeedbacktable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "CuisineType",
                table: "Restaurants",
                newName: "CuisineTypes");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Feedbacks",
                newName: "FoodRating");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CuisineType",
                table: "MenuItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryDriverRating",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryTimeRating",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuisineType",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "DeliveryDriverRating",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DeliveryTimeRating",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "CuisineTypes",
                table: "Restaurants",
                newName: "CuisineType");

            migrationBuilder.RenameColumn(
                name: "FoodRating",
                table: "Feedbacks",
                newName: "Rating");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
