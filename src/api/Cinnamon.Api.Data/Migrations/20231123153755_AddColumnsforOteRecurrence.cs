using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddColumnsforOteRecurrence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExtraOptions",
                table: "OteSchedules",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RecurrenceDateEnd",
                table: "OteSchedules",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RecurrenceDateStart",
                table: "OteSchedules",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RepeatEvery",
                table: "OteSchedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SelectedDays",
                table: "OteSchedules",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraOptions",
                table: "OteSchedules");

            migrationBuilder.DropColumn(
                name: "RecurrenceDateEnd",
                table: "OteSchedules");

            migrationBuilder.DropColumn(
                name: "RecurrenceDateStart",
                table: "OteSchedules");

            migrationBuilder.DropColumn(
                name: "RepeatEvery",
                table: "OteSchedules");

            migrationBuilder.DropColumn(
                name: "SelectedDays",
                table: "OteSchedules");
        }
    }
}
