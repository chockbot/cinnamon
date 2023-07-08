using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class updatedChatRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_FromUserId",
                table: "ChatRooms",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_ToUserId",
                table: "ChatRooms",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatHistories_ChatRoomId",
                table: "ChatHistories",
                column: "ChatRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatHistories_ChatRooms_ChatRoomId",
                table: "ChatHistories",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_Customers_CustomerId",
                table: "ChatRooms",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_Customers_FromUserId",
                table: "ChatRooms",
                column: "FromUserId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_Customers_ToUserId",
                table: "ChatRooms",
                column: "ToUserId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatHistories_ChatRooms_ChatRoomId",
                table: "ChatHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_Customers_CustomerId",
                table: "ChatRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_Customers_FromUserId",
                table: "ChatRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_Customers_ToUserId",
                table: "ChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_ChatRooms_CustomerId",
                table: "ChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_ChatRooms_FromUserId",
                table: "ChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_ChatRooms_ToUserId",
                table: "ChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_ChatHistories_ChatRoomId",
                table: "ChatHistories");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ChatRooms");
        }
    }
}
