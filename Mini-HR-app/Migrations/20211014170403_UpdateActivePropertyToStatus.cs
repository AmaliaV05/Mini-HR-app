using Microsoft.EntityFrameworkCore.Migrations;

namespace Mini_HR_app.Migrations
{
    public partial class UpdateActivePropertyToStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Active",
                table: "CompanyEmployee",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Companies",
                newName: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "CompanyEmployee",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Companies",
                newName: "Active");
        }
    }
}
