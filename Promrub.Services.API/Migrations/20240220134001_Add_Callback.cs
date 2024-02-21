using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Callback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "payment_status",
                table: "PaymenTransactions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "receipt_amount",
                table: "PaymenTransactions",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "receipt_date",
                table: "PaymenTransactions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "receipt_no",
                table: "PaymenTransactions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentChannels",
                columns: table => new
                {
                    payment_channel_id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: true),
                    payment_channel_type = table.Column<int>(type: "integer", nullable: true),
                    bank_code = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true),
                    is_exist = table.Column<bool>(type: "boolean", nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentChannels", x => x.payment_channel_id);
                });

            migrationBuilder.CreateTable(
                name: "RecieptNo",
                columns: table => new
                {
                    reciept_id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: true),
                    reciept_date = table.Column<string>(type: "text", nullable: true),
                    allocated = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecieptNo", x => x.reciept_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentChannels");

            migrationBuilder.DropTable(
                name: "RecieptNo");

            migrationBuilder.DropColumn(
                name: "payment_status",
                table: "PaymenTransactions");

            migrationBuilder.DropColumn(
                name: "receipt_amount",
                table: "PaymenTransactions");

            migrationBuilder.DropColumn(
                name: "receipt_date",
                table: "PaymenTransactions");

            migrationBuilder.DropColumn(
                name: "receipt_no",
                table: "PaymenTransactions");
        }
    }
}
