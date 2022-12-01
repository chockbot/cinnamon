using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinnamon.Data.Migrations
{
    public partial class UpdateResendEmailTableRemoveCountColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "ResendEmails");

            migrationBuilder.CreateIndex(
                name: "IX_ResendEmails_Email_DateResend",
                table: "ResendEmails",
                columns: new[] { "Email", "DateResend" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ResendEmails_Email_DateResend",
                table: "ResendEmails");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "ResendEmails",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
