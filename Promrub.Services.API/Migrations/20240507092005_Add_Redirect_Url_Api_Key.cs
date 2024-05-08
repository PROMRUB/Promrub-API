using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Redirect_Url_Api_Key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "redirect_url",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "api_Key",
                table: "PaymenTransactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "redirect_url",
                table: "ApiKeys",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "api_Key",
                table: "PaymenTransactions");

            migrationBuilder.DropColumn(
                name: "redirect_url",
                table: "ApiKeys");

            migrationBuilder.AddColumn<string>(
                name: "redirect_url",
                table: "Organizations",
                type: "text",
                nullable: true);
        }
    }
}
