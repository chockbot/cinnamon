using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddPoliciesinDescriptionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalData",
                table: "ActivityDescriptions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClassPolicies",
                table: "ActivityDescriptions",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalData",
                table: "ActivityDescriptions");

            migrationBuilder.DropColumn(
                name: "ClassPolicies",
                table: "ActivityDescriptions");
        }
    }
}
