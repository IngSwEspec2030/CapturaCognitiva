using Microsoft.EntityFrameworkCore.Migrations;

namespace CapturaCognitiva.Data.Migrations
{
    public partial class imagenes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Imagenestests",
                columns: table => new
                {
                    iD = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenestests", x => x.iD);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Imagenestests");
        }
    }
}
