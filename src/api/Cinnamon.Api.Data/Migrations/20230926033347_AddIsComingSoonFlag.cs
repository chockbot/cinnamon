using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddIsComingSoonFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Activities_ExperienceCreationTypeId",
                table: "Activities");

            migrationBuilder.AddColumn<bool>(
                name: "IsComingSoon",
                table: "Activities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ExperienceCreationTypeId",
                table: "Activities",
                column: "ExperienceCreationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Activities_ExperienceCreationTypeId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "IsComingSoon",
                table: "Activities");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ExperienceCreationTypeId",
                table: "Activities",
                column: "ExperienceCreationTypeId");
        }
    }
}
