using Microsoft.EntityFrameworkCore.Migrations;

namespace Mini_HR_app.Migrations
{
    public partial class UpdatePhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a2c4f2b-8ac9-492a-822a-47b173ebd7e0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b8ba58b4-9896-403d-a01e-265d528d132e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1585c92-7d06-4011-9e73-71b9f4aaccfb");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "Photos");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4f519548-c2ee-4c49-9c0d-93a8ea493913", "40486f9e-ac1a-4ff8-8e61-14129e7dfeca", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9f04932a-3e40-4221-ae72-3de609383815", "f6301c52-5234-416a-994e-90aeee5e5448", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "13265f41-12fe-434a-bd29-260e08887979", "84ae0941-30bf-495b-a4e2-eb4e8fb1399d", "Member", "MEMBER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13265f41-12fe-434a-bd29-260e08887979");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f519548-c2ee-4c49-9c0d-93a8ea493913");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f04932a-3e40-4221-ae72-3de609383815");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "Photos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b8ba58b4-9896-403d-a01e-265d528d132e", "8ec9c9b0-e7cb-4b76-81da-450ea077e155", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c1585c92-7d06-4011-9e73-71b9f4aaccfb", "880e7a10-4dbe-4b64-8e4d-6a48ce623800", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7a2c4f2b-8ac9-492a-822a-47b173ebd7e0", "978a8e48-572e-494f-aeaf-5553e7abb114", "Member", "MEMBER" });
        }
    }
}
