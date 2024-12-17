using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class AddFullTaxNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FullTaxNo",
                columns: table => new
                {
                    reciept_id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<string>(type: "text", nullable: true),
                    org_code = table.Column<string>(type: "text", nullable: true),
                    branch_code = table.Column<string>(type: "text", nullable: true),
                    cashier_code = table.Column<string>(type: "text", nullable: true),
                    employee_code = table.Column<string>(type: "text", nullable: true),
                    reciept_date = table.Column<string>(type: "text", nullable: true),
                    allocated = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullTaxNo", x => x.reciept_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FullTaxNo");
        }
    }
}
