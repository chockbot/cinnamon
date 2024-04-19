using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class ActivitySummaryIndexForReviewAccumulater : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ActivitySummaries_ReviewAccumulated",
                table: "ActivitySummaries",
                column: "ReviewAccumulated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActivitySummaries_ReviewAccumulated",
                table: "ActivitySummaries");
        }
    }
}
