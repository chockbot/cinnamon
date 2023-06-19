using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class UpdateStudentAttendanceAddHasReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasReview",
                table: "StudentAttendances",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasReview",
                table: "StudentAttendances");
        }
    }
}
