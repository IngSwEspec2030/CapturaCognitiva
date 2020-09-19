using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CapturaCognitiva.Data.Migrations
{
    public partial class seAñadeFinalizaciondesubidaaService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaUpload",
                table: "Guides",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpload",
                table: "Guides",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaUpload",
                table: "Guides");

            migrationBuilder.DropColumn(
                name: "IsUpload",
                table: "Guides");
        }
    }
}
