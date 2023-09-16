using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class addedChatMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_Customers_FromUserId",
                table: "ChatRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_Customers_ToUserId",
                table: "ChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_ChatRooms_FromUserId",
                table: "ChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_ChatRooms_ToUserId",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "ChatRooms");

            migrationBuilder.AddColumn<string>(
                name: "LatestMessage",
                table: "ChatRooms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ChatRooms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ChatMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatRoomId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    HasLeft = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMembers_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMembers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatHistories_FromUserId",
                table: "ChatHistories",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatHistories_ToUserId",
                table: "ChatHistories",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMembers_ChatRoomId",
                table: "ChatMembers",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMembers_CustomerId",
                table: "ChatMembers",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatHistories_Customers_FromUserId",
                table: "ChatHistories",
                column: "FromUserId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatHistories_Customers_ToUserId",
                table: "ChatHistories",
                column: "ToUserId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatHistories_Customers_FromUserId",
                table: "ChatHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatHistories_Customers_ToUserId",
                table: "ChatHistories");

            migrationBuilder.DropTable(
                name: "ChatMembers");

            migrationBuilder.DropIndex(
                name: "IX_ChatHistories_FromUserId",
                table: "ChatHistories");

            migrationBuilder.DropIndex(
                name: "IX_ChatHistories_ToUserId",
                table: "ChatHistories");

            migrationBuilder.DropColumn(
                name: "LatestMessage",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ChatRooms");

            migrationBuilder.AddColumn<int>(
                name: "FromUserId",
                table: "ChatRooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToUserId",
                table: "ChatRooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_FromUserId",
                table: "ChatRooms",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_ToUserId",
                table: "ChatRooms",
                column: "ToUserId");

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
    }
}
