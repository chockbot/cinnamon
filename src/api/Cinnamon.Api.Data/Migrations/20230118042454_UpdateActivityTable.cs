using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class UpdateActivityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barangay",
                table: "ActivityAddress",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "ActivityAddress",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "ActivityAddress",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subdivision",
                table: "ActivityAddress",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barangay",
                table: "ActivityAddress");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "ActivityAddress");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "ActivityAddress");

            migrationBuilder.DropColumn(
                name: "Subdivision",
                table: "ActivityAddress");
        }
    }
}
