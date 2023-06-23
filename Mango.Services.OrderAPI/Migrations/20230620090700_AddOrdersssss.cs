using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.OrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdersssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_CartHeaderDto_CartHeaderId",
                table: "OrderDetails");

            migrationBuilder.DropTable(
                name: "CartHeaderDto");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_CartHeaderId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "CartHeaderId",
                table: "OrderDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartHeaderId",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CartHeaderDto",
                columns: table => new
                {
                    CartHeaderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartTotal = table.Column<double>(type: "float", nullable: false),
                    CouponCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartHeaderDto", x => x.CartHeaderId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CartHeaderId",
                table: "OrderDetails",
                column: "CartHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_CartHeaderDto_CartHeaderId",
                table: "OrderDetails",
                column: "CartHeaderId",
                principalTable: "CartHeaderDto",
                principalColumn: "CartHeaderId");
        }
    }
}
