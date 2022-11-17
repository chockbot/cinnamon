using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Data.Migrations
{
    public partial class updateWaitListTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "Type",
                table: "WaitLists");

            migrationBuilder.AddColumn<bool>(
                name: "AcceptFlag",
                table: "Customers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_Email",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "AcceptFlag",
                table: "Customers");

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

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "WaitLists",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
