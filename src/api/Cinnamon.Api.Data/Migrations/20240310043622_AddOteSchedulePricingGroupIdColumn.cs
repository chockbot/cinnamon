using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddOteSchedulePricingGroupIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OteOnlineEvent_OteSchedulePricingGroups_OteSchedulePricingG~",
                table: "OteOnlineEvent");

            migrationBuilder.DropIndex(
                name: "IX_OteOnlineEvent_OteSchedulePricingGroupId",
                table: "OteOnlineEvent");

            migrationBuilder.AddColumn<int>(
                name: "OteOnlineEventId",
                table: "OteSchedulePricings",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OteSchedulePricingGroupId",
                table: "OteOnlineEvent",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OteSchedulePricings_OteOnlineEventId",
                table: "OteSchedulePricings",
                column: "OteOnlineEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_OteSchedulePricings_OteOnlineEvent_OteOnlineEventId",
                table: "OteSchedulePricings",
                column: "OteOnlineEventId",
                principalTable: "OteOnlineEvent",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OteSchedulePricings_OteOnlineEvent_OteOnlineEventId",
                table: "OteSchedulePricings");

            migrationBuilder.DropIndex(
                name: "IX_OteSchedulePricings_OteOnlineEventId",
                table: "OteSchedulePricings");

            migrationBuilder.DropColumn(
                name: "OteOnlineEventId",
                table: "OteSchedulePricings");

            migrationBuilder.AlterColumn<int>(
                name: "OteSchedulePricingGroupId",
                table: "OteOnlineEvent",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_OteOnlineEvent_OteSchedulePricingGroupId",
                table: "OteOnlineEvent",
                column: "OteSchedulePricingGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_OteOnlineEvent_OteSchedulePricingGroups_OteSchedulePricingG~",
                table: "OteOnlineEvent",
                column: "OteSchedulePricingGroupId",
                principalTable: "OteSchedulePricingGroups",
                principalColumn: "Id");
        }
    }
}
