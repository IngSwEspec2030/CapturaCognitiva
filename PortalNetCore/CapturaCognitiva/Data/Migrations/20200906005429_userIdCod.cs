using Microsoft.EntityFrameworkCore.Migrations;

namespace CapturaCognitiva.Data.Migrations
{
    public partial class userIdCod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "CodigoForgotPasswords",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodigoForgotPasswords_ApplicationUserId",
                table: "CodigoForgotPasswords",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodigoForgotPasswords_AspNetUsers_ApplicationUserId",
                table: "CodigoForgotPasswords",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodigoForgotPasswords_AspNetUsers_ApplicationUserId",
                table: "CodigoForgotPasswords");

            migrationBuilder.DropIndex(
                name: "IX_CodigoForgotPasswords_ApplicationUserId",
                table: "CodigoForgotPasswords");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "CodigoForgotPasswords");
        }
    }
}
