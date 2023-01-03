using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddExperienceCategoryColumnInActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExperienceCategoryId",
                table: "Activities",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "Activities",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ExperienceCategoryId",
                table: "Activities",
                column: "ExperienceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_SubCategoryId",
                table: "Activities",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_ExperienceCategories_ExperienceCategoryId",
                table: "Activities",
                column: "ExperienceCategoryId",
                principalTable: "ExperienceCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_SubCategory_SubCategoryId",
                table: "Activities",
                column: "SubCategoryId",
                principalTable: "SubCategory",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ExperienceCategories_ExperienceCategoryId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_SubCategory_SubCategoryId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ExperienceCategoryId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_SubCategoryId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ExperienceCategoryId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Activities");
        }
    }
}
