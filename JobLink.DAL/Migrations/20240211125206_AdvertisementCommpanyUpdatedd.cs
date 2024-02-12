using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobLink.DAL.Migrations
{
    public partial class AdvertisementCommpanyUpdatedd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Advertisements");
        }
    }
}
