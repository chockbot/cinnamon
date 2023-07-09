using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class UpdateCouponTableRule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Activities_ActivityId",
                table: "Coupons");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Activities_ActivityId",
                table: "Coupons",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Activities_ActivityId",
                table: "Coupons");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Activities_ActivityId",
                table: "Coupons",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
