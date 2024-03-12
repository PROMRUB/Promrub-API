using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Callback_Organization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "authorization_type",
                table: "Organizations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "callback_url",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "security_credential",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "security_password",
                table: "Organizations",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "authorization_type",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "callback_url",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "security_credential",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "security_password",
                table: "Organizations");
        }
    }
}
