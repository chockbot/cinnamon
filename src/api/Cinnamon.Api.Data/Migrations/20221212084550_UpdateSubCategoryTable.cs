using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class UpdateSubCategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategory_ExperienceCategories_ExperienceCategoryId",
                table: "SubCategory");

            migrationBuilder.DropIndex(
                name: "IX_SubCategory_ExperienceCategoryId",
                table: "SubCategory");

            migrationBuilder.DropColumn(
                name: "ExperienceCategoryId",
                table: "SubCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExperienceCategoryId",
                table: "SubCategory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategory_ExperienceCategoryId",
                table: "SubCategory",
                column: "ExperienceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategory_ExperienceCategories_ExperienceCategoryId",
                table: "SubCategory",
                column: "ExperienceCategoryId",
                principalTable: "ExperienceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
