using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Item_Code_and_Saler : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "item_code",
                table: "PaymentTransactionItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "saler",
                table: "PaymenTransactions",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "item_code",
                table: "PaymentTransactionItems");

            migrationBuilder.DropColumn(
                name: "saler",
                table: "PaymenTransactions");
        }
    }
}
