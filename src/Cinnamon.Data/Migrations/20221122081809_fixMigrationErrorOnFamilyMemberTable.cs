using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Data.Migrations
{
    public partial class fixMigrationErrorOnFamilyMemberTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthYear",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "FamilyMembers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
