using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class UpdateePOTableForAddOns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AddOnsAmount",
                table: "PurchaseOrders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_AddOns_ActivityId",
                table: "AddOns",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddOns_Activities_ActivityId",
                table: "AddOns",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddOns_Activities_ActivityId",
                table: "AddOns");

            migrationBuilder.DropIndex(
                name: "IX_AddOns_ActivityId",
                table: "AddOns");

            migrationBuilder.DropColumn(
                name: "AddOnsAmount",
                table: "PurchaseOrders");
        }
    }
}
