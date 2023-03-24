using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class addedIsDeactivated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactivated",
                table: "Activities",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactivated",
                table: "Activities");
        }
    }
}
