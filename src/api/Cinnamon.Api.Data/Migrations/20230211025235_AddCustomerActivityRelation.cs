using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddCustomerActivityRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Activities_CreatedBy",
                table: "Activities",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Customers_CreatedBy",
                table: "Activities",
                column: "CreatedBy",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Customers_CreatedBy",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_CreatedBy",
                table: "Activities");
        }
    }
}
