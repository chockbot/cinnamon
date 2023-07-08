using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class addedChatRoomRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_Customers_CustomerId",
                table: "ChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_ChatRooms_CustomerId",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ChatRooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "ChatRooms",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_CustomerId",
                table: "ChatRooms",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_Customers_CustomerId",
                table: "ChatRooms",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
