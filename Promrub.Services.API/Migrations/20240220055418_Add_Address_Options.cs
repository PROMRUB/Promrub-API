using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promrub.Services.API.Migrations
{
    public partial class Add_Address_Options : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    district_id = table.Column<Guid>(type: "uuid", nullable: false),
                    province_code = table.Column<int>(type: "integer", nullable: true),
                    district_code = table.Column<int>(type: "integer", nullable: true),
                    district_name_en = table.Column<string>(type: "text", nullable: true),
                    district_name_th = table.Column<string>(type: "text", nullable: true),
                    postal_code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.district_id);
                });

            migrationBuilder.CreateTable(
                name: "SubDistricts",
                columns: table => new
                {
                    sub_district_id = table.Column<Guid>(type: "uuid", nullable: false),
                    province_code = table.Column<int>(type: "integer", nullable: true),
                    district_code = table.Column<int>(type: "integer", nullable: true),
                    sub_district_code = table.Column<int>(type: "integer", nullable: true),
                    sub_district_name_en = table.Column<string>(type: "text", nullable: true),
                    sub_district_name_th = table.Column<string>(type: "text", nullable: true),
                    postal_code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDistricts", x => x.sub_district_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "SubDistricts");
        }
    }
}
