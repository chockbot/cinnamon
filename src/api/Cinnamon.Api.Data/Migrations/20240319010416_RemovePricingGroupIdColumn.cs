using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class RemovePricingGroupIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "OteSchedulePricingGroupId",
                table: "OteOnlineEvent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OteOnlineEventId",
                table: "OteSchedulePricings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OteSchedulePricingGroupId",
                table: "OteOnlineEvent",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
    }
}
