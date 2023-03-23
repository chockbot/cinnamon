using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class UpdateBadgeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfStudent",
                table: "BadgeList",
                newName: "NumberOfEnrolledStudent");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCompletedStudent",
                table: "BadgeList",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfCompletedStudent",
                table: "BadgeList");

            migrationBuilder.RenameColumn(
                name: "NumberOfEnrolledStudent",
                table: "BadgeList",
                newName: "NumberOfStudent");
        }
    }
}
