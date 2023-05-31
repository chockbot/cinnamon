using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddCustomerToCustomerPricingRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerPricings_CustomerId",
                table: "CustomerPricings");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPricings_CustomerId",
                table: "CustomerPricings",
                column: "CustomerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerPricings_CustomerId",
                table: "CustomerPricings");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPricings_CustomerId",
                table: "CustomerPricings",
                column: "CustomerId");
        }
    }
}
