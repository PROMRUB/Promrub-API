using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Discount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "discount",
                table: "PaymentTransactionItems",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "grand_total",
                table: "PaymentTransactionItems",
                type: "numeric",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discount",
                table: "PaymentTransactionItems");

            migrationBuilder.DropColumn(
                name: "grand_total",
                table: "PaymentTransactionItems");
        }
    }
}
