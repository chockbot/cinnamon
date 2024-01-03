using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddOtePricingsGroupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OteSchedulePricingGroupId",
                table: "OteSchedulePricings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OteSchedulePricingGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OteScheduleId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxSlots = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsAbsorbFees = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TicketSold = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OteSchedulePricingGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OteSchedulePricingGroup_OteSchedules_OteScheduleId",
                        column: x => x.OteScheduleId,
                        principalTable: "OteSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OteSchedulePricings_OteSchedulePricingGroupId",
                table: "OteSchedulePricings",
                column: "OteSchedulePricingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_OteSchedulePricingGroup_OteScheduleId",
                table: "OteSchedulePricingGroup",
                column: "OteScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_OteSchedulePricings_OteSchedulePricingGroup_OteSchedulePric~",
                table: "OteSchedulePricings",
                column: "OteSchedulePricingGroupId",
                principalTable: "OteSchedulePricingGroup",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OteSchedulePricings_OteSchedulePricingGroup_OteSchedulePric~",
                table: "OteSchedulePricings");

            migrationBuilder.DropTable(
                name: "OteSchedulePricingGroup");

            migrationBuilder.DropIndex(
                name: "IX_OteSchedulePricings_OteSchedulePricingGroupId",
                table: "OteSchedulePricings");

            migrationBuilder.DropColumn(
                name: "OteSchedulePricingGroupId",
                table: "OteSchedulePricings");
        }
    }
}
