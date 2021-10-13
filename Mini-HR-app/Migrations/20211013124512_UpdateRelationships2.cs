using Microsoft.EntityFrameworkCore.Migrations;

namespace Mini_HR_app.Migrations
{
    public partial class UpdateRelationships2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_People_PersonId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PersonId",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "CompanyPerson",
                columns: table => new
                {
                    CompaniesId = table.Column<int>(type: "int", nullable: false),
                    PeopleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPerson", x => new { x.CompaniesId, x.PeopleId });
                    table.ForeignKey(
                        name: "FK_CompanyPerson_Companies_CompaniesId",
                        column: x => x.CompaniesId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyPerson_People_PeopleId",
                        column: x => x.PeopleId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PersonId",
                table: "Employees",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPerson_PeopleId",
                table: "CompanyPerson",
                column: "PeopleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_People_PersonId",
                table: "Employees",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_People_PersonId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "CompanyPerson");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PersonId",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PersonId",
                table: "Employees",
                column: "PersonId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_People_PersonId",
                table: "Employees",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
