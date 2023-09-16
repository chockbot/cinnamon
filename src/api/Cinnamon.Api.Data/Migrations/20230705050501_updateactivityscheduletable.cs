using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class updateactivityscheduletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDateEnd",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ExpirationDateStart",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsSetSession",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "SessionName",
                table: "Activities");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDateEnd",
                table: "ActivitySchedules",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDateStart",
                table: "ActivitySchedules",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "HasExpiration",
                table: "ActivitySchedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSetSession",
                table: "ActivitySchedules",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SessionName",
                table: "ActivitySchedules",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDateEnd",
                table: "ActivitySchedules");

            migrationBuilder.DropColumn(
                name: "ExpirationDateStart",
                table: "ActivitySchedules");

            migrationBuilder.DropColumn(
                name: "HasExpiration",
                table: "ActivitySchedules");

            migrationBuilder.DropColumn(
                name: "IsSetSession",
                table: "ActivitySchedules");

            migrationBuilder.DropColumn(
                name: "SessionName",
                table: "ActivitySchedules");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDateEnd",
                table: "Students",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDateStart",
                table: "Students",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsSetSession",
                table: "Activities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SessionName",
                table: "Activities",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
