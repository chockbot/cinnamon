using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class AddSearchTagsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivitySearchTags");

            migrationBuilder.RenameColumn(
                name: "Subcategory",
                table: "SubCategory",
                newName: "SubCatergory");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "SubCategory",
                newName: "CatergoryId");

            migrationBuilder.CreateTable(
                name: "SearchTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    SearchTag1 = table.Column<string>(type: "text", nullable: false),
                    SearchTag2 = table.Column<string>(type: "text", nullable: false),
                    SearchTag3 = table.Column<string>(type: "text", nullable: false),
                    SearchTag4 = table.Column<string>(type: "text", nullable: false),
                    SearchTag5 = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchTags_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SearchTags_ActivityId",
                table: "SearchTags",
                column: "ActivityId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchTags");

            migrationBuilder.RenameColumn(
                name: "SubCatergory",
                table: "SubCategory",
                newName: "Subcategory");

            migrationBuilder.RenameColumn(
                name: "CatergoryId",
                table: "SubCategory",
                newName: "CategoryId");

            migrationBuilder.CreateTable(
                name: "ActivitySearchTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SearchTag1 = table.Column<string>(type: "text", nullable: true),
                    SearchTag2 = table.Column<string>(type: "text", nullable: true),
                    SearchTag3 = table.Column<string>(type: "text", nullable: true),
                    SearchTag4 = table.Column<string>(type: "text", nullable: true),
                    SearchTag5 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitySearchTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivitySearchTags_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySearchTags_ActivityId",
                table: "ActivitySearchTags",
                column: "ActivityId",
                unique: true);
        }
    }
}
