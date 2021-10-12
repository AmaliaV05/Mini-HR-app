using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mini_HR_app.Migrations
{
    public partial class AddCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FiscalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistryNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Euid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfEstablishment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NaceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
