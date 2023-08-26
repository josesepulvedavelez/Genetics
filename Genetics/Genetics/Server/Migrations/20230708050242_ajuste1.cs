using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Genetics.Server.Migrations
{
    /// <inheritdoc />
    public partial class ajuste1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Freight",
                table: "OrderDetail");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderDetail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderDetail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalFreight",
                table: "Order",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "TotalFreight",
                table: "Order");

            migrationBuilder.AddColumn<decimal>(
                name: "Freight",
                table: "OrderDetail",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
