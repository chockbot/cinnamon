using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class UpdateStudentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasReview",
                table: "StudentAttendances");

            migrationBuilder.AddColumn<bool>(
                name: "HasReview",
                table: "Students",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasReview",
                table: "Students");

            migrationBuilder.AddColumn<bool>(
                name: "HasReview",
                table: "StudentAttendances",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
