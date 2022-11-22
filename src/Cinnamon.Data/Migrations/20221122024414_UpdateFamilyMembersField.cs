using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Data.Migrations
{
    public partial class UpdateFamilyMembersField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMembers_Customers_CustomerModelId",
                table: "FamilyMembers");

            migrationBuilder.DropIndex(
                name: "IX_FamilyMembers_CustomerModelId",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "CustomerModelId",
                table: "FamilyMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMembers_Customers_CustomerId",
                table: "FamilyMembers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMembers_Customers_CustomerId",
                table: "FamilyMembers");

            migrationBuilder.AddColumn<int>(
                name: "CustomerModelId",
                table: "FamilyMembers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_CustomerModelId",
                table: "FamilyMembers",
                column: "CustomerModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMembers_Customers_CustomerModelId",
                table: "FamilyMembers",
                column: "CustomerModelId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
