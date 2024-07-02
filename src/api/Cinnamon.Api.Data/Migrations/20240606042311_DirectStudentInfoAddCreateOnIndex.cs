using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class DirectStudentInfoAddCreateOnIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DirectStudentInfos_CreatedOn",
                table: "DirectStudentInfos",
                column: "CreatedOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DirectStudentInfos_CreatedOn",
                table: "DirectStudentInfos");
        }
    }
}
