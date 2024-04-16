using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddTotalParticipantsColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalParticipants",
                table: "ActivitySummaries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySummaries_Ongoing_Completed_TotalParticipants",
                table: "ActivitySummaries",
                columns: new[] { "Ongoing", "Completed", "TotalParticipants" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySummaries_TotalParticipants",
                table: "ActivitySummaries",
                column: "TotalParticipants");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActivitySummaries_Ongoing_Completed_TotalParticipants",
                table: "ActivitySummaries");

            migrationBuilder.DropIndex(
                name: "IX_ActivitySummaries_TotalParticipants",
                table: "ActivitySummaries");

            migrationBuilder.DropColumn(
                name: "TotalParticipants",
                table: "ActivitySummaries");
        }
    }
}
