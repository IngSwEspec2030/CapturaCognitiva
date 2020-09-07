using Microsoft.EntityFrameworkCore.Migrations;

namespace CapturaCognitiva.Data.Migrations
{
    public partial class cambiosIdentificadores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cell",
                table: "Senders",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cell",
                table: "Receivers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Uuid",
                table: "Images",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cell",
                table: "Senders");

            migrationBuilder.DropColumn(
                name: "Cell",
                table: "Receivers");

            migrationBuilder.AlterColumn<int>(
                name: "Uuid",
                table: "Images",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
