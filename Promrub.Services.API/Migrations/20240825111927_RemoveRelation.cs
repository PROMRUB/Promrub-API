using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class RemoveRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxReceipt_PaymenTransactions_payment_transaction",
                table: "TaxReceipt");

            migrationBuilder.DropIndex(
                name: "IX_TaxReceipt_payment_transaction",
                table: "TaxReceipt");

            migrationBuilder.DropColumn(
                name: "payment_transaction",
                table: "TaxReceipt");

            migrationBuilder.AddColumn<Guid>(
                name: "TaxScheduleEntityTaxId",
                table: "TaxReceipt",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxReceipt_TaxScheduleEntityTaxId",
                table: "TaxReceipt",
                column: "TaxScheduleEntityTaxId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxReceipt_TaxScheduleEntity_TaxScheduleEntityTaxId",
                table: "TaxReceipt",
                column: "TaxScheduleEntityTaxId",
                principalTable: "TaxScheduleEntity",
                principalColumn: "tax_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxReceipt_TaxScheduleEntity_TaxScheduleEntityTaxId",
                table: "TaxReceipt");

            migrationBuilder.DropIndex(
                name: "IX_TaxReceipt_TaxScheduleEntityTaxId",
                table: "TaxReceipt");

            migrationBuilder.DropColumn(
                name: "TaxScheduleEntityTaxId",
                table: "TaxReceipt");

            migrationBuilder.AddColumn<Guid>(
                name: "payment_transaction",
                table: "TaxReceipt",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TaxReceipt_payment_transaction",
                table: "TaxReceipt",
                column: "payment_transaction");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxReceipt_PaymenTransactions_payment_transaction",
                table: "TaxReceipt",
                column: "payment_transaction",
                principalTable: "PaymenTransactions",
                principalColumn: "payment_transaction_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
