using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Provinces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Provices",
                columns: table => new
                {
                    provice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provice_code = table.Column<int>(type: "integer", nullable: true),
                    province_name_en = table.Column<string>(type: "text", nullable: true),
                    province_name_th = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provices", x => x.provice_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Provices");
        }
    }
}
