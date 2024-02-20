using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Organization_Details : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "branch_id",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "district",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "house_no",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "post_code",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "provice",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "road",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sub_district",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tax_id",
                table: "Organizations",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "branch_id",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "district",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "house_no",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "post_code",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "provice",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "road",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "sub_district",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "tax_id",
                table: "Organizations");
        }
    }
}
