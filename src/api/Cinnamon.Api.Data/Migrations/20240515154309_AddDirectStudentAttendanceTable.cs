using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddDirectStudentAttendanceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DirectStudentAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DirectStudentSessionId = table.Column<int>(type: "integer", nullable: false),
                    IsPresent = table.Column<bool>(type: "boolean", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectStudentAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DirectStudentAttendances_DirectStudentSessions_DirectStuden~",
                        column: x => x.DirectStudentSessionId,
                        principalTable: "DirectStudentSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DirectStudentAttendances_Date_DirectStudentSessionId",
                table: "DirectStudentAttendances",
                columns: new[] { "Date", "DirectStudentSessionId" });

            migrationBuilder.CreateIndex(
                name: "IX_DirectStudentAttendances_DirectStudentSessionId",
                table: "DirectStudentAttendances",
                column: "DirectStudentSessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DirectStudentAttendances");
        }
    }
}
