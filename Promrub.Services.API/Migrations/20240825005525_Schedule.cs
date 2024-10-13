using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Schedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceiptScheduleEntity",
                columns: table => new
                {
                    receipt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    receipt_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pos_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptScheduleEntity", x => x.receipt_id);
                });

            migrationBuilder.CreateTable(
                name: "TaxScheduleEntity",
                columns: table => new
                {
                    tax_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tax_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pos_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    vat = table.Column<decimal>(type: "numeric", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    total_receipt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxScheduleEntity", x => x.tax_id);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptReceipt",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    receipt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    receipt_no = table.Column<string>(type: "text", nullable: false),
                    payment_transaction = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptReceipt", x => x.id);
                    table.ForeignKey(
                        name: "FK_ReceiptReceipt_PaymenTransactions_payment_transaction",
                        column: x => x.payment_transaction,
                        principalTable: "PaymenTransactions",
                        principalColumn: "payment_transaction_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptReceipt_ReceiptScheduleEntity_receipt_id",
                        column: x => x.receipt_id,
                        principalTable: "ReceiptScheduleEntity",
                        principalColumn: "receipt_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxReceipt",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tax_id = table.Column<Guid>(type: "uuid", nullable: false),
                    receipt_no = table.Column<string>(type: "text", nullable: false),
                    total_receipt = table.Column<string>(type: "text", nullable: false),
                    payment_transaction = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxReceipt", x => x.id);
                    table.ForeignKey(
                        name: "FK_TaxReceipt_PaymenTransactions_payment_transaction",
                        column: x => x.payment_transaction,
                        principalTable: "PaymenTransactions",
                        principalColumn: "payment_transaction_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxReceipt_TaxScheduleEntity_tax_id",
                        column: x => x.tax_id,
                        principalTable: "TaxScheduleEntity",
                        principalColumn: "tax_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptReceipt_payment_transaction",
                table: "ReceiptReceipt",
                column: "payment_transaction");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptReceipt_receipt_id",
                table: "ReceiptReceipt",
                column: "receipt_id");

            migrationBuilder.CreateIndex(
                name: "IX_TaxReceipt_payment_transaction",
                table: "TaxReceipt",
                column: "payment_transaction");

            migrationBuilder.CreateIndex(
                name: "IX_TaxReceipt_tax_id",
                table: "TaxReceipt",
                column: "tax_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptReceipt");

            migrationBuilder.DropTable(
                name: "TaxReceipt");

            migrationBuilder.DropTable(
                name: "ReceiptScheduleEntity");

            migrationBuilder.DropTable(
                name: "TaxScheduleEntity");
        }
    }
}
