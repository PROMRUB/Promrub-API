using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Discount_And_Biller_Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "percentage",
                table: "PaymentTransactionItems",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_discount",
                table: "PaymentTransactionItems",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "biller_id",
                table: "PaymentChannels",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "percentage",
                table: "PaymentTransactionItems");

            migrationBuilder.DropColumn(
                name: "total_discount",
                table: "PaymentTransactionItems");

            migrationBuilder.DropColumn(
                name: "biller_id",
                table: "PaymentChannels");
        }
    }
}
