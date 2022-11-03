using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Data.Migrations
{
    public partial class AddedNewColumnWaitingList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AcceptFlag",
                table: "WaitLists",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Birthdate",
                table: "WaitLists",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "WaitLists",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "WaitLists",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "WaitLists",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "WaitLists",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WaitLists_Guid",
                table: "WaitLists",
                column: "Guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WaitLists_Guid",
                table: "WaitLists");

            migrationBuilder.DropColumn(
                name: "AcceptFlag",
                table: "WaitLists");

            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "WaitLists");

            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "WaitLists");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "WaitLists");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "WaitLists");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "WaitLists");
        }
    }
}
