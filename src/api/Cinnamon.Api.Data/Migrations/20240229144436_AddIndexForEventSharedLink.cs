using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddIndexForEventSharedLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OteSharedLinks_ActivityId_OteDateId",
                table: "OteSharedLinks",
                columns: new[] { "ActivityId", "OteDateId" });

            migrationBuilder.CreateIndex(
                name: "IX_OteSharedLinks_Guid_Token",
                table: "OteSharedLinks",
                columns: new[] { "Guid", "Token" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OteSharedLinks_ActivityId_OteDateId",
                table: "OteSharedLinks");

            migrationBuilder.DropIndex(
                name: "IX_OteSharedLinks_Guid_Token",
                table: "OteSharedLinks");
        }
    }
}
