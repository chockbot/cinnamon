using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddLocationAndProviderFieldInActivitySummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ActivitySummaries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "ActivitySummaries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySummaries_Location",
                table: "ActivitySummaries",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySummaries_Provider",
                table: "ActivitySummaries",
                column: "Provider");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActivitySummaries_Location",
                table: "ActivitySummaries");

            migrationBuilder.DropIndex(
                name: "IX_ActivitySummaries_Provider",
                table: "ActivitySummaries");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ActivitySummaries");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "ActivitySummaries");
        }
    }
}
