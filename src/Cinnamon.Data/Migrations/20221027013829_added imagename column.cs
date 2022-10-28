using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Data.Migrations
{
    public partial class addedimagenamecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "ActivityImages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityImages_ImageId",
                table: "ActivityImages",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityImages_Activities_ImageId",
                table: "ActivityImages",
                column: "ImageId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityImages_Activities_ImageId",
                table: "ActivityImages");

            migrationBuilder.DropIndex(
                name: "IX_ActivityImages_ImageId",
                table: "ActivityImages");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "ActivityImages");
        }
    }
}
