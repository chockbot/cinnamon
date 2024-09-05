using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class SchedulePricingsAddReserveSeatUUid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReserveSeatUuid",
                table: "OteSchedulePricings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReserveSeatUuid",
                table: "OteSchedulePricingGroups",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReserveSeatUuid",
                table: "OteSchedulePricings");

            migrationBuilder.DropColumn(
                name: "ReserveSeatUuid",
                table: "OteSchedulePricingGroups");
        }
    }
}
