using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class PurchaseOrderUpdateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_ActivitySchedules_ScheduleId",
                table: "PurchaseOrders");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "PurchaseOrders",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_ActivitySchedules_ScheduleId",
                table: "PurchaseOrders",
                column: "ScheduleId",
                principalTable: "ActivitySchedules",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_ActivitySchedules_ScheduleId",
                table: "PurchaseOrders");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "PurchaseOrders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_ActivitySchedules_ScheduleId",
                table: "PurchaseOrders",
                column: "ScheduleId",
                principalTable: "ActivitySchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
