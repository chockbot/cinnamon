using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddSessionExpiration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSetSession",
                table: "ActivitySchedules");

            migrationBuilder.DropColumn(
                name: "SessionName",
                table: "ActivitySchedules");

            migrationBuilder.AddColumn<bool>(
                name: "IsSetSession",
                table: "Activities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SessionName",
                table: "Activities",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSetSession",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "SessionName",
                table: "Activities");

            migrationBuilder.AddColumn<bool>(
                name: "IsSetSession",
                table: "ActivitySchedules",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SessionName",
                table: "ActivitySchedules",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
