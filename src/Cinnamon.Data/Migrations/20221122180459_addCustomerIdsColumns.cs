using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Data.Migrations
{
    public partial class addCustomerIdsColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackIdImagePath",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontIdImagePath",
                table: "Customers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackIdImagePath",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FrontIdImagePath",
                table: "Customers");
        }
    }
}
