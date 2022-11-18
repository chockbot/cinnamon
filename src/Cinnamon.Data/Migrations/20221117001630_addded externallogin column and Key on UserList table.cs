using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Data.Migrations
{
    public partial class adddedexternallogincolumnandKeyonUserListtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserList",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "ExternalLogin",
                table: "UserList",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalLogin",
                table: "UserList");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserList",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
