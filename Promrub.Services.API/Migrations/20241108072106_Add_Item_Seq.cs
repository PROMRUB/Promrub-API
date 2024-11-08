using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Item_Seq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "seq",
                table: "PaymentTransactionItems",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "seq",
                table: "PaymentTransactionItems");
        }
    }
}
