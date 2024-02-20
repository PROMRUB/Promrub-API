using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_PaymentTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymenTransactions",
                columns: table => new
                {
                    payment_transaction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: true),
                    pos_id = table.Column<string>(type: "text", nullable: true),
                    transaction_id = table.Column<string>(type: "text", nullable: true),
                    item_total = table.Column<int>(type: "integer", nullable: false),
                    quantity_total = table.Column<int>(type: "integer", nullable: false),
                    totalItems_prices = table.Column<decimal>(type: "numeric", nullable: false),
                    total_discount = table.Column<decimal>(type: "numeric", nullable: false),
                    total_transaction_prices = table.Column<decimal>(type: "numeric", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymenTransactions", x => x.payment_transaction_id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactionItems",
                columns: table => new
                {
                    payment_transaction_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: true),
                    payment_transaction_id = table.Column<Guid>(type: "uuid", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    item_name = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: true),
                    total_prices = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactionItems", x => x.payment_transaction_item_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymenTransactions");

            migrationBuilder.DropTable(
                name: "PaymentTransactionItems");
        }
    }
}
