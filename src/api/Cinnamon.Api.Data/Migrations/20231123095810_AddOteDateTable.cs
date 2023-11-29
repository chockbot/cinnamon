using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddOteDateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OteDateId",
                table: "OteTickets",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OteDateId",
                table: "OteSchedulePricings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OteDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OteScheduleId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OteDates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OteTickets_OteDateId",
                table: "OteTickets",
                column: "OteDateId");

            migrationBuilder.CreateIndex(
                name: "IX_OteSchedulePricings_OteDateId",
                table: "OteSchedulePricings",
                column: "OteDateId");

            migrationBuilder.AddForeignKey(
                name: "FK_OteSchedulePricings_OteDates_OteDateId",
                table: "OteSchedulePricings",
                column: "OteDateId",
                principalTable: "OteDates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OteTickets_OteDates_OteDateId",
                table: "OteTickets",
                column: "OteDateId",
                principalTable: "OteDates",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OteSchedulePricings_OteDates_OteDateId",
                table: "OteSchedulePricings");

            migrationBuilder.DropForeignKey(
                name: "FK_OteTickets_OteDates_OteDateId",
                table: "OteTickets");

            migrationBuilder.DropTable(
                name: "OteDates");

            migrationBuilder.DropIndex(
                name: "IX_OteTickets_OteDateId",
                table: "OteTickets");

            migrationBuilder.DropIndex(
                name: "IX_OteSchedulePricings_OteDateId",
                table: "OteSchedulePricings");

            migrationBuilder.DropColumn(
                name: "OteDateId",
                table: "OteTickets");

            migrationBuilder.DropColumn(
                name: "OteDateId",
                table: "OteSchedulePricings");
        }
    }
}
