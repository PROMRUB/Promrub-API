using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class ReceiptNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptReceipt_PaymenTransactions_payment_transaction",
                table: "ReceiptReceipt");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptReceipt_payment_transaction",
                table: "ReceiptReceipt");

            migrationBuilder.DropColumn(
                name: "payment_transaction",
                table: "ReceiptReceipt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TaxReceipt",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "pos_id",
                table: "ReceiptScheduleEntity",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "receive_no",
                table: "ReceiptScheduleEntity",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiptScheduleEntityReceiptId",
                table: "ReceiptReceipt",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptReceipt_ReceiptScheduleEntityReceiptId",
                table: "ReceiptReceipt",
                column: "ReceiptScheduleEntityReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptReceipt_ReceiptScheduleEntity_ReceiptScheduleEntityR~",
                table: "ReceiptReceipt",
                column: "ReceiptScheduleEntityReceiptId",
                principalTable: "ReceiptScheduleEntity",
                principalColumn: "receipt_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptReceipt_ReceiptScheduleEntity_ReceiptScheduleEntityR~",
                table: "ReceiptReceipt");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptReceipt_ReceiptScheduleEntityReceiptId",
                table: "ReceiptReceipt");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TaxReceipt");

            migrationBuilder.DropColumn(
                name: "receive_no",
                table: "ReceiptScheduleEntity");

            migrationBuilder.DropColumn(
                name: "ReceiptScheduleEntityReceiptId",
                table: "ReceiptReceipt");

            migrationBuilder.AlterColumn<Guid>(
                name: "pos_id",
                table: "ReceiptScheduleEntity",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "payment_transaction",
                table: "ReceiptReceipt",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptReceipt_payment_transaction",
                table: "ReceiptReceipt",
                column: "payment_transaction");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptReceipt_PaymenTransactions_payment_transaction",
                table: "ReceiptReceipt",
                column: "payment_transaction",
                principalTable: "PaymenTransactions",
                principalColumn: "payment_transaction_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
