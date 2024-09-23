using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddOptionsInCustomQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "ProviderCustomQuestions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Options",
                table: "ProviderCustomQuestions");
        }
    }
}
