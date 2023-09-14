using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinnamon.Api.Data.Migrations
{
    public partial class rebuildmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    EmailAddress = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BadgeList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    NumberOfEnrolledStudent = table.Column<int>(type: "integer", nullable: false),
                    NumberOfCompletedStudent = table.Column<int>(type: "integer", nullable: false),
                    NumberOfReviews = table.Column<int>(type: "integer", nullable: false),
                    ImgScr = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Barangays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CityCode = table.Column<string>(type: "text", nullable: false),
                    MunicipalityCode = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barangays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LatestMessage = table.Column<string>(type: "text", nullable: false),
                    ChatType = table.Column<int>(type: "integer", nullable: false),
                    GroupName = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RegionCode = table.Column<string>(type: "text", nullable: false),
                    IsCity = table.Column<bool>(type: "boolean", nullable: false),
                    IsMunicipality = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    About = table.Column<string>(type: "text", nullable: true),
                    ProfilePath = table.Column<string>(type: "text", nullable: true),
                    DateJoined = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsMaker = table.Column<bool>(type: "boolean", nullable: false),
                    ExternalLogin = table.Column<bool>(type: "boolean", nullable: false),
                    IsVerifiedBadge = table.Column<int>(type: "integer", nullable: false),
                    IsVerifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsOG = table.Column<bool>(type: "boolean", nullable: false),
                    IsOGDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsOfficialPartner = table.Column<bool>(type: "boolean", nullable: false),
                    IsOfficialDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FrontIdImagePath = table.Column<string>(type: "text", nullable: true),
                    BackIdImagePath = table.Column<string>(type: "text", nullable: true),
                    Handler = table.Column<string>(type: "text", nullable: false),
                    TotalCredits = table.Column<decimal>(type: "numeric", nullable: false),
                    HasAcceptedTerms = table.Column<bool>(type: "boolean", nullable: false),
                    ConnectionId = table.Column<string>(type: "text", nullable: true),
                    IsAccountBan = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: false),
                    IconPath = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceCreationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "ExperienceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalLoginTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Guid = table.Column<string>(type: "text", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    DateGenerated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsEmptyUsername = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalLoginTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FailedLogins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Metadata = table.Column<string>(type: "text", nullable: false),
                    LoginDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedLogins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayoutAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    AccountNumber = table.Column<string>(type: "text", nullable: false),
                    AccountHolder = table.Column<string>(type: "text", nullable: false),
                    Payloads = table.Column<string>(type: "text", nullable: false),
                    BankChannel = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayoutAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayoutLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseOrderId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayoutLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RegionName = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResendEmails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    DateResend = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResendEmails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResetPasswords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Guid = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    GeneratedToken = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetPasswords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CatergoryId = table.Column<int>(type: "integer", nullable: false),
                    SubCatergory = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaitLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Guid = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    ConnectionId = table.Column<string>(type: "text", nullable: false),
                    UserAgent = table.Column<string>(type: "text", nullable: false),
                    IsConnected = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatConnections_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatRoomId = table.Column<int>(type: "integer", nullable: false),
                    FromUserId = table.Column<int>(type: "integer", nullable: false),
                    ToUserId = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    IsViewed = table.Column<bool>(type: "boolean", nullable: false),
                    FromConnectionId = table.Column<string>(type: "text", nullable: false),
                    ToConnectionId = table.Column<string>(type: "text", nullable: false),
                    ChatHistoryType = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatHistories_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatHistories_Customers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatHistories_Customers_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatRoomId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    HasLeft = table.Column<bool>(type: "boolean", nullable: false),
                    ChatMemberType = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMembers_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMembers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPricings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric", nullable: false),
                    IsManualPayment = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPricings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilyMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    BirthMonth = table.Column<string>(type: "text", nullable: false),
                    BirthYear = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExperienceTypeId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Subtitle = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<string>(type: "text", nullable: false),
                    ScheduleIndicator = table.Column<string>(type: "text", nullable: false),
                    MapDetails = table.Column<string>(type: "text", nullable: false),
                    Guarantee = table.Column<string>(type: "text", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    ExperienceCategoryId = table.Column<int>(type: "integer", nullable: true),
                    SubCategoryId = table.Column<int>(type: "integer", nullable: true),
                    Handler = table.Column<string>(type: "text", nullable: false),
                    IsNew = table.Column<bool>(type: "boolean", nullable: false),
                    PurchaseOrderCount = table.Column<int>(type: "integer", nullable: false),
                    IsDeactivated = table.Column<bool>(type: "boolean", nullable: false),
                    Guid = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExperienceCreationTypeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Customers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_ExperienceCategories_ExperienceCategoryId",
                        column: x => x.ExperienceCategoryId,
                        principalTable: "ExperienceCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Activities_ExperienceCreationTypes_ExperienceCreationTypeId",
                        column: x => x.ExperienceCreationTypeId,
                        principalTable: "ExperienceCreationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_ExperienceTypes_ExperienceTypeId",
                        column: x => x.ExperienceTypeId,
                        principalTable: "ExperienceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_SubCategory_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActivityAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    Address1 = table.Column<string>(type: "text", nullable: false),
                    Address2 = table.Column<string>(type: "text", nullable: false),
                    District = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    CityName = table.Column<string>(type: "text", nullable: false),
                    Subdivision = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    RegionName = table.Column<string>(type: "text", nullable: false),
                    Barangay = table.Column<string>(type: "text", nullable: false),
                    BarangayName = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    PinnedLocation = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityAddress_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    SpecificsYouWillProvide = table.Column<string>(type: "text", nullable: false),
                    CustomerBringWithThem = table.Column<string>(type: "text", nullable: false),
                    AdditionalRequirements = table.Column<string>(type: "text", nullable: true),
                    ActivityLevel = table.Column<string>(type: "text", nullable: false),
                    SkillLevel = table.Column<string>(type: "text", nullable: false),
                    MinimumAge = table.Column<int>(type: "integer", nullable: false),
                    CanAdultsJoin = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityDescriptions_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    ImageName = table.Column<string>(type: "text", nullable: false),
                    ImageLocation = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityImages_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivitySchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DateTime = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitPrice = table.Column<string>(type: "text", nullable: false),
                    PerUnit1 = table.Column<int>(type: "integer", nullable: false),
                    PriceUnit1 = table.Column<string>(type: "text", nullable: false),
                    PerUnit2 = table.Column<int>(type: "integer", nullable: false),
                    PriceUnit2 = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IsActiveSchedule = table.Column<bool>(type: "boolean", nullable: false),
                    IsSetSession = table.Column<bool>(type: "boolean", nullable: false),
                    SessionName = table.Column<string>(type: "text", nullable: false),
                    HasExpiration = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ScheduleType = table.Column<int>(type: "integer", nullable: false),
                    PriceType = table.Column<int>(type: "integer", nullable: false),
                    SchedulingUrl = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitySchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivitySchedules_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: true),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    DiscountType = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    MaximumSpend = table.Column<decimal>(type: "numeric", nullable: false),
                    From = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    To = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coupons_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Coupons_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OteSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    From = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    To = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Recurrences = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OteSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OteSchedules_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    MakerId = table.Column<int>(type: "integer", nullable: false),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Review = table.Column<string>(type: "text", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    SearchTag1 = table.Column<string>(type: "text", nullable: true),
                    SearchTag2 = table.Column<string>(type: "text", nullable: true),
                    SearchTag3 = table.Column<string>(type: "text", nullable: true),
                    SearchTag4 = table.Column<string>(type: "text", nullable: true),
                    SearchTag5 = table.Column<string>(type: "text", nullable: true),
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
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "OngoingActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OngoingActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OngoingActivities_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OngoingActivities_ActivitySchedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "ActivitySchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OngoingActivities_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    ConvinienceFee = table.Column<decimal>(type: "numeric", nullable: false),
                    Coupon = table.Column<string>(type: "text", nullable: true),
                    CouponAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    OverallTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    CreditAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitCount = table.Column<int>(type: "integer", nullable: false),
                    IsInclusivePayment = table.Column<bool>(type: "boolean", nullable: false),
                    PerUnitDisburseAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalDisburseAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_ActivitySchedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "ActivitySchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    FamilyMemberId = table.Column<int>(type: "integer", nullable: false),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StudentNo = table.Column<string>(type: "text", nullable: false),
                    NumberOfSessions = table.Column<int>(type: "integer", nullable: false),
                    NumberOfBacktracking = table.Column<int>(type: "integer", nullable: false),
                    SessionsAttended = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ExpirationDateStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpirationDateEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OngoingActivityId = table.Column<int>(type: "integer", nullable: false),
                    IsDisbursement = table.Column<bool>(type: "boolean", nullable: false),
                    HasReview = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_ActivitySchedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "ActivitySchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OteSchedulePricings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OteScheduleId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxSlots = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsAbsorbFees = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OteSchedulePricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OteSchedulePricings_OteSchedules_OteScheduleId",
                        column: x => x.OteScheduleId,
                        principalTable: "OteSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OngoingActivityScheduleTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityScheduleTimeId = table.Column<int>(type: "integer", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    ScheduleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "RequestRefunds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "integer", nullable: false),
                    ExperienceTitle = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    RefundAmountGiven = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestRefunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestRefunds_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestRefunds_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    IsPresent = table.Column<bool>(type: "boolean", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAttendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CreatedBy",
                table: "Activities",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ExperienceCategoryId",
                table: "Activities",
                column: "ExperienceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ExperienceCreationTypeId",
                table: "Activities",
                column: "ExperienceCreationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ExperienceTypeId",
                table: "Activities",
                column: "ExperienceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_Handler",
                table: "Activities",
                column: "Handler");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_IsDeactivated",
                table: "Activities",
                column: "IsDeactivated");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_IsNew",
                table: "Activities",
                column: "IsNew");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_IsPublished",
                table: "Activities",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_PurchaseOrderCount",
                table: "Activities",
                column: "PurchaseOrderCount");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_PurchaseOrderCount_IsPublished_IsDeactivated_IsN~",
                table: "Activities",
                columns: new[] { "PurchaseOrderCount", "IsPublished", "IsDeactivated", "IsNew" });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_SubCategoryId",
                table: "Activities",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAddress_ActivityId",
                table: "ActivityAddress",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDescriptions_ActivityId",
                table: "ActivityDescriptions",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityImages_ActivityId",
                table: "ActivityImages",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySchedules_ActivityId",
                table: "ActivitySchedules",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityScheduleTimes_ActivityScheduleId",
                table: "ActivityScheduleTimes",
                column: "ActivityScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BadgeList_Id",
                table: "BadgeList",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatConnections_CustomerId",
                table: "ChatConnections",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatHistories_ChatRoomId",
                table: "ChatHistories",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatHistories_FromUserId",
                table: "ChatHistories",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatHistories_ToUserId",
                table: "ChatHistories",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMembers_ChatRoomId",
                table: "ChatMembers",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMembers_CustomerId",
                table: "ChatMembers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_ActivityId",
                table: "Coupons",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_Code",
                table: "Coupons",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_Code_CustomerId_ActivityId",
                table: "Coupons",
                columns: new[] { "Code", "CustomerId", "ActivityId" });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_CustomerId",
                table: "Coupons",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPricings_CustomerId",
                table: "CustomerPricings",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Handler",
                table: "Customers",
                column: "Handler");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalLoginTokens_Token",
                table: "ExternalLoginTokens",
                column: "Token");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalLoginTokens_Token_Guid",
                table: "ExternalLoginTokens",
                columns: new[] { "Token", "Guid" });

            migrationBuilder.CreateIndex(
                name: "IX_FailedLogins_Email_LoginDate",
                table: "FailedLogins",
                columns: new[] { "Email", "LoginDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_CustomerId",
                table: "FamilyMembers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OngoingActivities_ActivityId",
                table: "OngoingActivities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_OngoingActivities_CustomerId",
                table: "OngoingActivities",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OngoingActivities_PurchaseOrderId",
                table: "OngoingActivities",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OngoingActivities_ScheduleId",
                table: "OngoingActivities",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_OngoingActivityScheduleTimes_ActivityScheduleTimeId",
                table: "OngoingActivityScheduleTimes",
                column: "ActivityScheduleTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_OteSchedulePricings_OteScheduleId",
                table: "OteSchedulePricings",
                column: "OteScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_OteSchedules_ActivityId",
                table: "OteSchedules",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayoutAccounts_CustomerId",
                table: "PayoutAccounts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ActivityId",
                table: "PurchaseOrders",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ScheduleId",
                table: "PurchaseOrders",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_Status",
                table: "PurchaseOrders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RequestRefunds_CustomerId",
                table: "RequestRefunds",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestRefunds_PurchaseOrderId",
                table: "RequestRefunds",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ResendEmails_Email",
                table: "ResendEmails",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_ResendEmails_Email_DateResend",
                table: "ResendEmails",
                columns: new[] { "Email", "DateResend" });

            migrationBuilder.CreateIndex(
                name: "IX_ResetPasswords_Guid_Token",
                table: "ResetPasswords",
                columns: new[] { "Guid", "Token" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ActivityId",
                table: "Reviews",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Id",
                table: "Reviews",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SearchTags_ActivityId",
                table: "SearchTags",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_StudentId",
                table: "StudentAttendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ActivityId",
                table: "Students",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CustomerId",
                table: "Students",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_FamilyMemberId",
                table: "Students",
                column: "FamilyMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_OngoingActivityId",
                table: "Students",
                column: "OngoingActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ScheduleId",
                table: "Students",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitLists_Guid",
                table: "WaitLists",
                column: "Guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityAddress");

            migrationBuilder.DropTable(
                name: "ActivityDescriptions");

            migrationBuilder.DropTable(
                name: "ActivityImages");

            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BadgeList");

            migrationBuilder.DropTable(
                name: "Barangays");

            migrationBuilder.DropTable(
                name: "ChatConnections");

            migrationBuilder.DropTable(
                name: "ChatHistories");

            migrationBuilder.DropTable(
                name: "ChatMembers");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "CustomerPricings");

            migrationBuilder.DropTable(
                name: "ExternalLoginTokens");

            migrationBuilder.DropTable(
                name: "FailedLogins");

            migrationBuilder.DropTable(
                name: "FamilyMembers");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "OngoingActivities");

            migrationBuilder.DropTable(
                name: "OngoingActivityScheduleTimes");

            migrationBuilder.DropTable(
                name: "OteSchedulePricings");

            migrationBuilder.DropTable(
                name: "PayoutAccounts");

            migrationBuilder.DropTable(
                name: "PayoutLogs");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "RequestRefunds");

            migrationBuilder.DropTable(
                name: "ResendEmails");

            migrationBuilder.DropTable(
                name: "ResetPasswords");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "SearchTags");

            migrationBuilder.DropTable(
                name: "StudentAttendances");

            migrationBuilder.DropTable(
                name: "WaitLists");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.DropTable(
                name: "ActivityScheduleTimes");

            migrationBuilder.DropTable(
                name: "OteSchedules");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "ActivitySchedules");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "ExperienceCategories");

            migrationBuilder.DropTable(
                name: "ExperienceCreationTypes");

            migrationBuilder.DropTable(
                name: "ExperienceTypes");

            migrationBuilder.DropTable(
                name: "SubCategory");
        }
    }
}
