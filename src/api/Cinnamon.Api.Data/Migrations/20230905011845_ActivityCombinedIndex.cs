using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class ActivityCombinedIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Activities_PurchaseOrderCount_IsPublished_IsDeactivated_IsN~",
                table: "Activities",
                columns: new[] { "PurchaseOrderCount", "IsPublished", "IsDeactivated", "IsNew" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Activities_PurchaseOrderCount_IsPublished_IsDeactivated_IsN~",
                table: "Activities");
        }
    }
}
