using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddDirectStudentSessionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DirectStudentSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DirectStudentInfoId = table.Column<int>(type: "integer", nullable: false),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StudentNo = table.Column<string>(type: "text", nullable: false),
                    NumberOfSessions = table.Column<int>(type: "integer", nullable: false),
                    SessionsAttended = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectStudentSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DirectStudentSessions_DirectStudentInfos_DirectStudentInfoId",
                        column: x => x.DirectStudentInfoId,
                        principalTable: "DirectStudentInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DirectStudentSessions_ActivityId",
                table: "DirectStudentSessions",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectStudentSessions_DirectStudentInfoId",
                table: "DirectStudentSessions",
                column: "DirectStudentInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectStudentSessions_ScheduleId",
                table: "DirectStudentSessions",
                column: "ScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DirectStudentSessions");
        }
    }
}
