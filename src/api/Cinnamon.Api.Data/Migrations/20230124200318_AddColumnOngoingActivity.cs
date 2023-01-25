using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddColumnOngoingActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "OngoingActivities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OngoingActivities_ScheduleId",
                table: "OngoingActivities",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_OngoingActivities_ActivitySchedules_ScheduleId",
                table: "OngoingActivities",
                column: "ScheduleId",
                principalTable: "ActivitySchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OngoingActivities_ActivitySchedules_ScheduleId",
                table: "OngoingActivities");

            migrationBuilder.DropIndex(
                name: "IX_OngoingActivities_ScheduleId",
                table: "OngoingActivities");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "OngoingActivities");
        }
    }
}
