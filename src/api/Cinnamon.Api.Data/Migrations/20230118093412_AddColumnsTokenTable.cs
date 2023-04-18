using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddColumnsTokenTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ExternalLoginTokens",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "ExternalLoginTokens",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalLoginTokens_Token_Guid",
                table: "ExternalLoginTokens",
                columns: new[] { "Token", "Guid" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExternalLoginTokens_Token_Guid",
                table: "ExternalLoginTokens");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ExternalLoginTokens");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "ExternalLoginTokens");
        }
    }
}
