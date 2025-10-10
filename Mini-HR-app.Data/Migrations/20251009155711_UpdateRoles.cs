using Microsoft.EntityFrameworkCore.Migrations;

namespace Mini_HR_app.Migrations
{
    public partial class UpdateRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "230e6437-35ee-447f-87a1-85640ee3fe46");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a4ef5c85-18c9-4da1-b877-ed0bcd7a2c16");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e661c653-0300-416c-b350-b59b9caa7800");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e4afbe47-571b-45a8-8019-25431dadf80d", "9d428364-6364-4f17-8f43-3269d7e2fc50", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1148e8f6-d753-42de-81d0-c2cf9662447b", "5927e66f-c510-4c85-89c3-397a79b60421", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "371a40a5-ea5b-4019-ab99-8660cf0b4dbc", "dce69349-7ec8-4ee9-bff4-b1c502552c7b", "Member", "MEMBER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1148e8f6-d753-42de-81d0-c2cf9662447b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "371a40a5-ea5b-4019-ab99-8660cf0b4dbc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e4afbe47-571b-45a8-8019-25431dadf80d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e661c653-0300-416c-b350-b59b9caa7800", "93bf26dc-89e5-4d91-b9fe-8b1ddcf07c65", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a4ef5c85-18c9-4da1-b877-ed0bcd7a2c16", "8dee4608-d8a4-4c35-9259-69acd95960a3", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "230e6437-35ee-447f-87a1-85640ee3fe46", "23b1fdb2-b353-4f18-a1d4-6705f44391a5", "Member", "MEMBER" });
        }
    }
}
