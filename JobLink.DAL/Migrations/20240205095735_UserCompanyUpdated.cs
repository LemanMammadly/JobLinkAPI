using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobLink.DAL.Migrations
{
    public partial class UserCompanyUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Companies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AppUserId",
                table: "Companies",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_AspNetUsers_AppUserId",
                table: "Companies",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_AspNetUsers_AppUserId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_AppUserId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Companies");
        }
    }
}
