using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class updatedChatHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ChatRooms",
                newName: "ToUserId");

            migrationBuilder.RenameColumn(
                name: "ChatRoomActivityId",
                table: "ChatRooms",
                newName: "FromUserId");

            migrationBuilder.RenameColumn(
                name: "ChatRoomActivityId",
                table: "ChatHistories",
                newName: "ChatRoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToUserId",
                table: "ChatRooms",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "FromUserId",
                table: "ChatRooms",
                newName: "ChatRoomActivityId");

            migrationBuilder.RenameColumn(
                name: "ChatRoomId",
                table: "ChatHistories",
                newName: "ChatRoomActivityId");
        }
    }
}
