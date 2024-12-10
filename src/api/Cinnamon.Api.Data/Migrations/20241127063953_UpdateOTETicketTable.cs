using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class UpdateOTETicketTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OteTickets_PurchaseOrders_PurchaseOrderId",
                table: "OteTickets");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseOrderId",
                table: "OteTickets",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_OteTickets_PurchaseOrders_PurchaseOrderId",
                table: "OteTickets",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OteTickets_PurchaseOrders_PurchaseOrderId",
                table: "OteTickets");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseOrderId",
                table: "OteTickets",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OteTickets_PurchaseOrders_PurchaseOrderId",
                table: "OteTickets",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
