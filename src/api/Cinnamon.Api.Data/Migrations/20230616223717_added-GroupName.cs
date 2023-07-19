using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class addedGroupName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatType",
                table: "ChatRooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "ChatRooms",
                type: "text",
                nullable: false,
                defaultValue: "");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Reviews_ActivityId",
            //    table: "Reviews",
            //    column: "ActivityId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Reviews_Activities_ActivityId",
            //    table: "Reviews",
            //    column: "ActivityId",
            //    principalTable: "Activities",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Activities_ActivityId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ActivityId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ChatType",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "ChatRooms");
        }
    }
}
