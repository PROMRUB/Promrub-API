using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Email_and_Website : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "website",
                table: "Organizations",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "website",
                table: "Organizations");
        }
    }
}
