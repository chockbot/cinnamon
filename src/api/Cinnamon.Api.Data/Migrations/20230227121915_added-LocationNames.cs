using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class addedLocationNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BarangayName",
                table: "ActivityAddress",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                table: "ActivityAddress",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "ActivityAddress",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarangayName",
                table: "ActivityAddress");

            migrationBuilder.DropColumn(
                name: "CityName",
                table: "ActivityAddress");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "ActivityAddress");
        }
    }
}
