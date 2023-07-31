using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class addedExperienceCreationTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "IsSetSession",
            //    table: "Activities");

            //migrationBuilder.DropColumn(
            //    name: "SessionName",
            //    table: "Activities");

            //migrationBuilder.AddColumn<bool>(
            //    name: "HasReview",
            //    table: "Students",
            //    type: "boolean",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsInclusivePayment",
            //    table: "PurchaseOrders",
            //    type: "boolean",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<decimal>(
            //    name: "PerUnitDisburseAmount",
            //    table: "PurchaseOrders",
            //    type: "numeric",
            //    nullable: false,
            //    defaultValue: 0m);

            //migrationBuilder.AddColumn<decimal>(
            //    name: "TotalDisburseAmount",
            //    table: "PurchaseOrders",
            //    type: "numeric",
            //    nullable: false,
            //    defaultValue: 0m);

            //migrationBuilder.AddColumn<string>(
            //    name: "ConnectionId",
            //    table: "Customers",
            //    type: "text",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "HasExpiration",
            //    table: "ActivitySchedules",
            //    type: "integer",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsSetSession",
            //    table: "ActivitySchedules",
            //    type: "boolean",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "SessionName",
            //    table: "ActivitySchedules",
            //    type: "text",
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "StartDate",
            //    table: "ActivitySchedules",
            //    type: "timestamp with time zone",
            //    nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceCreationTypeId",
                table: "Activities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ActivityScheduleTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityScheduleId = table.Column<int>(type: "integer", nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<string>(type: "text", nullable: false),
                    EndTime = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityScheduleTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityScheduleTimes_ActivitySchedules_ActivityScheduleId",
                        column: x => x.ActivityScheduleId,
                        principalTable: "ActivitySchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "ChatConnections",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        CustomerId = table.Column<int>(type: "integer", nullable: false),
            //        ConnectionId = table.Column<string>(type: "text", nullable: false),
            //        UserAgent = table.Column<string>(type: "text", nullable: false),
            //        IsConnected = table.Column<bool>(type: "boolean", nullable: false),
            //        CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        CreatedBy = table.Column<int>(type: "integer", nullable: false),
            //        ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        ChangedBy = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ChatConnections", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ChatConnections_Customers_CustomerId",
            //            column: x => x.CustomerId,
            //            principalTable: "Customers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ChatRooms",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Name = table.Column<string>(type: "text", nullable: false),
            //        LatestMessage = table.Column<string>(type: "text", nullable: false),
            //        ChatType = table.Column<int>(type: "integer", nullable: false),
            //        GroupName = table.Column<string>(type: "text", nullable: false),
            //        CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        CreatedBy = table.Column<int>(type: "integer", nullable: false),
            //        ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        ChangedBy = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ChatRooms", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Coupons",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        ActivityId = table.Column<int>(type: "integer", nullable: true),
            //        IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
            //        CustomerId = table.Column<int>(type: "integer", nullable: false),
            //        Name = table.Column<string>(type: "text", nullable: false),
            //        Code = table.Column<string>(type: "text", nullable: false),
            //        DiscountType = table.Column<int>(type: "integer", nullable: false),
            //        Amount = table.Column<decimal>(type: "numeric", nullable: false),
            //        MaximumSpend = table.Column<decimal>(type: "numeric", nullable: false),
            //        From = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        To = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        Status = table.Column<int>(type: "integer", nullable: false),
            //        CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        CreatedBy = table.Column<int>(type: "integer", nullable: false),
            //        ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        ChangedBy = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Coupons", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Coupons_Activities_ActivityId",
            //            column: x => x.ActivityId,
            //            principalTable: "Activities",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_Coupons_Customers_CustomerId",
            //            column: x => x.CustomerId,
            //            principalTable: "Customers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CustomerPricings",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        CustomerId = table.Column<int>(type: "integer", nullable: false),
            //        Email = table.Column<string>(type: "text", nullable: false),
            //        Rate = table.Column<decimal>(type: "numeric", nullable: false),
            //        IsManualPayment = table.Column<bool>(type: "boolean", nullable: false),
            //        CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        CreatedBy = table.Column<int>(type: "integer", nullable: false),
            //        ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        ChangedBy = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CustomerPricings", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_CustomerPricings_Customers_CustomerId",
            //            column: x => x.CustomerId,
            //            principalTable: "Customers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateTable(
                name: "ExperienceCreationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceCreationTypes", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "Favorites",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        CustomerId = table.Column<int>(type: "integer", nullable: false),
            //        ActivityId = table.Column<int>(type: "integer", nullable: false),
            //        CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        CreatedBy = table.Column<int>(type: "integer", nullable: false),
            //        ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        ChangedBy = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Favorites", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Reviews",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        CustomerId = table.Column<int>(type: "integer", nullable: false),
            //        MakerId = table.Column<int>(type: "integer", nullable: false),
            //        ActivityId = table.Column<int>(type: "integer", nullable: false),
            //        ScheduleId = table.Column<int>(type: "integer", nullable: false),
            //        StudentId = table.Column<int>(type: "integer", nullable: false),
            //        Rating = table.Column<int>(type: "integer", nullable: false),
            //        Review = table.Column<string>(type: "text", nullable: false),
            //        ReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        CreatedBy = table.Column<int>(type: "integer", nullable: false),
            //        ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        ChangedBy = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Reviews", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Reviews_Activities_ActivityId",
            //            column: x => x.ActivityId,
            //            principalTable: "Activities",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateTable(
                name: "OngoingActivityScheduleTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityScheduleTimeId = table.Column<int>(type: "integer", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OngoingActivityScheduleTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OngoingActivityScheduleTimes_ActivityScheduleTimes_Activity~",
                        column: x => x.ActivityScheduleTimeId,
                        principalTable: "ActivityScheduleTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "ChatHistories",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        ChatRoomId = table.Column<int>(type: "integer", nullable: false),
            //        FromUserId = table.Column<int>(type: "integer", nullable: false),
            //        ToUserId = table.Column<int>(type: "integer", nullable: false),
            //        Message = table.Column<string>(type: "text", nullable: false),
            //        IsViewed = table.Column<bool>(type: "boolean", nullable: false),
            //        FromConnectionId = table.Column<string>(type: "text", nullable: false),
            //        ToConnectionId = table.Column<string>(type: "text", nullable: false),
            //        ChatHistoryType = table.Column<int>(type: "integer", nullable: false),
            //        CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        CreatedBy = table.Column<int>(type: "integer", nullable: false),
            //        ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        ChangedBy = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ChatHistories", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ChatHistories_ChatRooms_ChatRoomId",
            //            column: x => x.ChatRoomId,
            //            principalTable: "ChatRooms",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ChatHistories_Customers_FromUserId",
            //            column: x => x.FromUserId,
            //            principalTable: "Customers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ChatHistories_Customers_ToUserId",
            //            column: x => x.ToUserId,
            //            principalTable: "Customers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ChatMembers",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        ChatRoomId = table.Column<int>(type: "integer", nullable: false),
            //        CustomerId = table.Column<int>(type: "integer", nullable: false),
            //        HasLeft = table.Column<bool>(type: "boolean", nullable: false),
            //        ChatMemberType = table.Column<int>(type: "integer", nullable: false),
            //        CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        CreatedBy = table.Column<int>(type: "integer", nullable: false),
            //        ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        ChangedBy = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ChatMembers", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ChatMembers_ChatRooms_ChatRoomId",
            //            column: x => x.ChatRoomId,
            //            principalTable: "ChatRooms",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ChatMembers_Customers_CustomerId",
            //            column: x => x.CustomerId,
            //            principalTable: "Customers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ExperienceCreationTypeId",
                table: "Activities",
                column: "ExperienceCreationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityScheduleTimes_ActivityScheduleId",
                table: "ActivityScheduleTimes",
                column: "ActivityScheduleId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ChatConnections_CustomerId",
            //    table: "ChatConnections",
            //    column: "CustomerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ChatHistories_ChatRoomId",
            //    table: "ChatHistories",
            //    column: "ChatRoomId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ChatHistories_FromUserId",
            //    table: "ChatHistories",
            //    column: "FromUserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ChatHistories_ToUserId",
            //    table: "ChatHistories",
            //    column: "ToUserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ChatMembers_ChatRoomId",
            //    table: "ChatMembers",
            //    column: "ChatRoomId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ChatMembers_CustomerId",
            //    table: "ChatMembers",
            //    column: "CustomerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Coupons_ActivityId",
            //    table: "Coupons",
            //    column: "ActivityId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Coupons_Code",
            //    table: "Coupons",
            //    column: "Code");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Coupons_Code_CustomerId_ActivityId",
            //    table: "Coupons",
            //    columns: new[] { "Code", "CustomerId", "ActivityId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Coupons_CustomerId",
            //    table: "Coupons",
            //    column: "CustomerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CustomerPricings_CustomerId",
            //    table: "CustomerPricings",
            //    column: "CustomerId",
            //    unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OngoingActivityScheduleTimes_ActivityScheduleTimeId",
                table: "OngoingActivityScheduleTimes",
                column: "ActivityScheduleTimeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Reviews_ActivityId",
            //    table: "Reviews",
            //    column: "ActivityId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Reviews_Id",
            //    table: "Reviews",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Activities_ExperienceCreationTypes_ExperienceCreationTypeId",
            //    table: "Activities",
            //    column: "ExperienceCreationTypeId",
            //    principalTable: "ExperienceCreationTypes",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ExperienceCreationTypes_ExperienceCreationTypeId",
                table: "Activities");

            migrationBuilder.DropTable(
                name: "ChatConnections");

            migrationBuilder.DropTable(
                name: "ChatHistories");

            migrationBuilder.DropTable(
                name: "ChatMembers");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "CustomerPricings");

            migrationBuilder.DropTable(
                name: "ExperienceCreationTypes");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "OngoingActivityScheduleTimes");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.DropTable(
                name: "ActivityScheduleTimes");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ExperienceCreationTypeId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "HasReview",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsInclusivePayment",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PerUnitDisburseAmount",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "TotalDisburseAmount",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "HasExpiration",
                table: "ActivitySchedules");

            migrationBuilder.DropColumn(
                name: "IsSetSession",
                table: "ActivitySchedules");

            migrationBuilder.DropColumn(
                name: "SessionName",
                table: "ActivitySchedules");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ActivitySchedules");

            migrationBuilder.DropColumn(
                name: "ExperienceCreationTypeId",
                table: "Activities");

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
