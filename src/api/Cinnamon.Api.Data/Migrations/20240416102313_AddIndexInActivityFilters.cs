using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddIndexInActivityFilters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Activities_IsDeactivated_Status_IsPublished_ForceDisable",
                table: "Activities",
                columns: new[] { "IsDeactivated", "Status", "IsPublished", "ForceDisable" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Activities_IsDeactivated_Status_IsPublished_ForceDisable",
                table: "Activities");
        }
    }
}
