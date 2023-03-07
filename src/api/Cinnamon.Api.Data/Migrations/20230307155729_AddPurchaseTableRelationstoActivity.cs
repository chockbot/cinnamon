using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddPurchaseTableRelationstoActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ActivityId",
                table: "PurchaseOrders",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ScheduleId",
                table: "PurchaseOrders",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Activities_ActivityId",
                table: "PurchaseOrders",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_ActivitySchedules_ScheduleId",
                table: "PurchaseOrders",
                column: "ScheduleId",
                principalTable: "ActivitySchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Activities_ActivityId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_ActivitySchedules_ScheduleId",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_ActivityId",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_ScheduleId",
                table: "PurchaseOrders");
        }
    }
}
