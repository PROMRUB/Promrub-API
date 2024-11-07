using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Full_Address_and_Tel_No : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "full_address",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tel_no",
                table: "Organizations",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "full_address",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "tel_no",
                table: "Organizations");
        }
    }
}
