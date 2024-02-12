using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobLink.DAL.Migrations
{
    public partial class AdvertisementCommpanyUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CompanyId",
                table: "Advertisements",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Companies_CompanyId",
                table: "Advertisements",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Companies_CompanyId",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_CompanyId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Advertisements");
        }
    }
}
