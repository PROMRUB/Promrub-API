using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Banks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    bank_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_code = table.Column<int>(type: "integer", nullable: true),
                    bank_abbr = table.Column<string>(type: "text", nullable: true),
                    bank_name_en = table.Column<string>(type: "text", nullable: true),
                    bank_name_th = table.Column<string>(type: "text", nullable: true),
                    bank_swift_code = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.bank_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bank");
        }
    }
}
