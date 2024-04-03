using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class Message : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OteOnlineEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OteScheduleId = table.Column<int>(type: "integer", nullable: false),
                    Videolink = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TicketRestriction = table.Column<string>(type: "text", nullable: false),
                    OteSchedulePricingGroupId = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OteOnlineEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OteOnlineEvent_OteSchedulePricingGroups_OteSchedulePricingG~",
                        column: x => x.OteSchedulePricingGroupId,
                        principalTable: "OteSchedulePricingGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OteOnlineEvent_OteSchedules_OteScheduleId",
                        column: x => x.OteScheduleId,
                        principalTable: "OteSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OteOnlineEvent_OteScheduleId",
                table: "OteOnlineEvent",
                column: "OteScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_OteOnlineEvent_OteSchedulePricingGroupId",
                table: "OteOnlineEvent",
                column: "OteSchedulePricingGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OteOnlineEvent");
        }
    }
}
