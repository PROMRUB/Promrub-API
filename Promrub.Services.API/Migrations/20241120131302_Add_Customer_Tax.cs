using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Customer_Tax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "customer_tax_id",
                table: "PaymenTransactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customer_Tax",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tax_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    tel = table.Column<string>(type: "text", nullable: false),
                    full_address = table.Column<string>(type: "text", nullable: false),
                    post_code = table.Column<string>(type: "text", nullable: false),
                    is_memo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer_Tax", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymenTransactions_customer_tax_id",
                table: "PaymenTransactions",
                column: "customer_tax_id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymenTransactions_Customer_Tax_customer_tax_id",
                table: "PaymenTransactions",
                column: "customer_tax_id",
                principalTable: "Customer_Tax",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymenTransactions_Customer_Tax_customer_tax_id",
                table: "PaymenTransactions");

            migrationBuilder.DropTable(
                name: "Customer_Tax");

            migrationBuilder.DropIndex(
                name: "IX_PaymenTransactions_customer_tax_id",
                table: "PaymenTransactions");

            migrationBuilder.DropColumn(
                name: "customer_tax_id",
                table: "PaymenTransactions");
        }
    }
}
