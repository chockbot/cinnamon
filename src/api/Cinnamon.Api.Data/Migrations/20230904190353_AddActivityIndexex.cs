using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddActivityIndexex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Activities_IsDeactivated",
                table: "Activities",
                column: "IsDeactivated");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_IsNew",
                table: "Activities",
                column: "IsNew");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_IsPublished",
                table: "Activities",
                column: "IsPublished");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Activities_IsDeactivated",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_IsNew",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_IsPublished",
                table: "Activities");
        }
    }
}
