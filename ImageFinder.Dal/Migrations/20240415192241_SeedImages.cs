using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageFinder.Dal.Migrations
{
    public partial class SeedImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Images",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "Url" },
                values: new object[] { 1, "https://api.dicebear.com/8.x/pixel-art/png?seed=" });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "Url" },
                values: new object[] { 2, "https://api.dicebear.com/8.x/pixel-art/png?seed=" });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "Url" },
                values: new object[] { 3, "https://api.dicebear.com/8.x/pixel-art/png?seed=" });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "Url" },
                values: new object[] { 4, "https://api.dicebear.com/8.x/pixel-art/png?seed=" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Images",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
