using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class UpdateOteScheduleReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OteDates_OteScheduleId",
                table: "OteDates",
                column: "OteScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_OteDates_OteSchedules_OteScheduleId",
                table: "OteDates",
                column: "OteScheduleId",
                principalTable: "OteSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OteDates_OteSchedules_OteScheduleId",
                table: "OteDates");

            migrationBuilder.DropIndex(
                name: "IX_OteDates_OteScheduleId",
                table: "OteDates");
        }
    }
}
