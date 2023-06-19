using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.ShoppingCartAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CartHeaders",
                newName: "CartHeaderId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CartDetails",
                newName: "CartDetailsId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CartHeaders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CartHeaderId",
                table: "CartHeaders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CartDetailsId",
                table: "CartDetails",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CartHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
