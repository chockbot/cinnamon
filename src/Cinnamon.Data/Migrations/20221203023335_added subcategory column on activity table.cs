using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Data.Migrations
{
    public partial class addedsubcategorycolumnonactivitytable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExperienceSubCategoryId",
                table: "Activities",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExperienceSubCategoryId",
                table: "Activities");
        }
    }
}
