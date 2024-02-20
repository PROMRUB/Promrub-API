using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Payment_Method_Options : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "hv_card",
                table: "Organizations",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "hv_mobile_banking",
                table: "Organizations",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "hv_promtpay",
                table: "Organizations",
                type: "boolean",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hv_card",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "hv_mobile_banking",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "hv_promtpay",
                table: "Organizations");
        }
    }
}
