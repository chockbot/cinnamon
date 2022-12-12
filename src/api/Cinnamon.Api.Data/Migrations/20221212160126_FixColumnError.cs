using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class FixColumnError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subcategory",
                table: "SubCategory",
                newName: "SubCatergory");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "SubCategory",
                newName: "CatergoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubCatergory",
                table: "SubCategory",
                newName: "Subcategory");

            migrationBuilder.RenameColumn(
                name: "CatergoryId",
                table: "SubCategory",
                newName: "CategoryId");
        }
    }
}
