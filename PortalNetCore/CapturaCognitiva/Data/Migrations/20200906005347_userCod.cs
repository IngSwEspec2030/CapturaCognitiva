using Microsoft.EntityFrameworkCore.Migrations;

namespace CapturaCognitiva.Data.Migrations
{
    public partial class userCod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "AspNetUsersId",
                table: "CodigoForgotPasswords");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "CodigoForgotPasswords",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AspNetUsersId",
                table: "CodigoForgotPasswords",
                type: "nvarchar(max)",
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
    }
}
