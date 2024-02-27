using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class TempRemoveAmountColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "DisbursementDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Amount",
                table: "DisbursementDetails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
