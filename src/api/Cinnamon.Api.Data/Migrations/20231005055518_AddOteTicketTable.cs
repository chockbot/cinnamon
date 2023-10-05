using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddOteTicketTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OteTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    OteScheduleId = table.Column<int>(type: "integer", nullable: false),
                    OteSchedulePricingId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    QRCode = table.Column<string>(type: "text", nullable: false),
                    QRImageData = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OteTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OteTickets_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OteTickets_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OteTickets_OteSchedulePricings_OteSchedulePricingId",
                        column: x => x.OteSchedulePricingId,
                        principalTable: "OteSchedulePricings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OteTickets_OteSchedules_OteScheduleId",
                        column: x => x.OteScheduleId,
                        principalTable: "OteSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OteTickets_ActivityId_QRCode_Status",
                table: "OteTickets",
                columns: new[] { "ActivityId", "QRCode", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_OteTickets_CustomerId",
                table: "OteTickets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OteTickets_OteScheduleId",
                table: "OteTickets",
                column: "OteScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_OteTickets_OteSchedulePricingId",
                table: "OteTickets",
                column: "OteSchedulePricingId");

            migrationBuilder.CreateIndex(
                name: "IX_OteTickets_QRCode",
                table: "OteTickets",
                column: "QRCode");

            migrationBuilder.CreateIndex(
                name: "IX_OteTickets_Status",
                table: "OteTickets",
                column: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OteTickets");
        }
    }
}
