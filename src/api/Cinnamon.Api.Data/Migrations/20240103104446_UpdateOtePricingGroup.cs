using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class UpdateOtePricingGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OteSchedulePricingGroup_OteSchedules_OteScheduleId",
                table: "OteSchedulePricingGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_OteSchedulePricings_OteSchedulePricingGroup_OteSchedulePric~",
                table: "OteSchedulePricings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OteSchedulePricingGroup",
                table: "OteSchedulePricingGroup");

            migrationBuilder.RenameTable(
                name: "OteSchedulePricingGroup",
                newName: "OteSchedulePricingGroups");

            migrationBuilder.RenameIndex(
                name: "IX_OteSchedulePricingGroup_OteScheduleId",
                table: "OteSchedulePricingGroups",
                newName: "IX_OteSchedulePricingGroups_OteScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OteSchedulePricingGroups",
                table: "OteSchedulePricingGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OteSchedulePricingGroups_OteSchedules_OteScheduleId",
                table: "OteSchedulePricingGroups",
                column: "OteScheduleId",
                principalTable: "OteSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OteSchedulePricings_OteSchedulePricingGroups_OteSchedulePri~",
                table: "OteSchedulePricings",
                column: "OteSchedulePricingGroupId",
                principalTable: "OteSchedulePricingGroups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OteSchedulePricingGroups_OteSchedules_OteScheduleId",
                table: "OteSchedulePricingGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_OteSchedulePricings_OteSchedulePricingGroups_OteSchedulePri~",
                table: "OteSchedulePricings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OteSchedulePricingGroups",
                table: "OteSchedulePricingGroups");

            migrationBuilder.RenameTable(
                name: "OteSchedulePricingGroups",
                newName: "OteSchedulePricingGroup");

            migrationBuilder.RenameIndex(
                name: "IX_OteSchedulePricingGroups_OteScheduleId",
                table: "OteSchedulePricingGroup",
                newName: "IX_OteSchedulePricingGroup_OteScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OteSchedulePricingGroup",
                table: "OteSchedulePricingGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OteSchedulePricingGroup_OteSchedules_OteScheduleId",
                table: "OteSchedulePricingGroup",
                column: "OteScheduleId",
                principalTable: "OteSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OteSchedulePricings_OteSchedulePricingGroup_OteSchedulePric~",
                table: "OteSchedulePricings",
                column: "OteSchedulePricingGroupId",
                principalTable: "OteSchedulePricingGroup",
                principalColumn: "Id");
        }
    }
}
