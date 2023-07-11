using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class addPurchaseOrderDisburseFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PerUnitDisburseAmount",
                table: "PurchaseOrders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDisburseAmount",
                table: "PurchaseOrders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerUnitDisburseAmount",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "TotalDisburseAmount",
                table: "PurchaseOrders");
        }
    }
}
