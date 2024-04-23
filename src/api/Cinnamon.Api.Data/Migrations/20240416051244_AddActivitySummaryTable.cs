using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddActivitySummaryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivitySummaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    ImageBannerSrc = table.Column<string>(type: "text", nullable: false),
                    Ongoing = table.Column<int>(type: "integer", nullable: false),
                    Completed = table.Column<int>(type: "integer", nullable: false),
                    TotalReviews = table.Column<int>(type: "integer", nullable: false),
                    ReviewAccumulated = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitySummaries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySummaries_ActivityId",
                table: "ActivitySummaries",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySummaries_Ongoing",
                table: "ActivitySummaries",
                column: "Ongoing");

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySummaries_Ongoing_Completed",
                table: "ActivitySummaries",
                columns: new[] { "Ongoing", "Completed" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivitySummaries");
        }
    }
}
