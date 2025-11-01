using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentServicesWebApi.Migrations
{
    /// <inheritdoc />
    public partial class JBHbhbfuenf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OpenaiKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UseCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenaiKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TextSlides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    Size = table.Column<int>(type: "integer", nullable: false),
                    Font = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsBold = table.Column<bool>(type: "boolean", nullable: false),
                    IsItalic = table.Column<bool>(type: "boolean", nullable: false),
                    ColorHex = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    Left = table.Column<double>(type: "double precision", precision: 18, scale: 6, nullable: false),
                    Top = table.Column<double>(type: "double precision", precision: 18, scale: 6, nullable: false),
                    Width = table.Column<double>(type: "double precision", precision: 18, scale: 6, nullable: false),
                    Height = table.Column<double>(type: "double precision", precision: 18, scale: 6, nullable: true),
                    Horizontal = table.Column<int>(type: "integer", nullable: false),
                    Vertical = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextSlides", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    TelegramId = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Photo = table.Column<string>(type: "text", nullable: true),
                    Balance = table.Column<int>(type: "integer", nullable: false),
                    ReferralId = table.Column<int>(type: "integer", nullable: true),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlanTextId = table.Column<int>(type: "integer", nullable: false),
                    PlansId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plans_TextSlides_PlanTextId",
                        column: x => x.PlanTextId,
                        principalTable: "TextSlides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plans_TextSlides_PlansId",
                        column: x => x.PlansId,
                        principalTable: "TextSlides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdminId = table.Column<int>(type: "integer", nullable: false),
                    TargetUserId = table.Column<int>(type: "integer", nullable: false),
                    ActionType = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    PreviousValue = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NewValue = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NotificationSent = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminActions_Users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdminActions_Users_TargetUserId",
                        column: x => x.TargetUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Designs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Designs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ActionUrl = table.Column<string>(type: "text", nullable: true),
                    IconUrl = table.Column<string>(type: "text", nullable: true),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsGlobal = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderId = table.Column<int>(type: "integer", nullable: false),
                    RequestedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Photo = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ApprovedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    PaymentStatus = table.Column<int>(type: "integer", nullable: false),
                    RejectReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ProcessedByAdminId = table.Column<int>(type: "integer", nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AdminNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Users_ProcessedByAdminId",
                        column: x => x.ProcessedByAdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VerificationCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    TelegramDeepLink = table.Column<string>(type: "text", nullable: true),
                    CodeType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerificationCodes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoSlides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhotoPath = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    OriginalFileName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Left = table.Column<double>(type: "double precision", precision: 18, scale: 6, nullable: false),
                    Top = table.Column<double>(type: "double precision", precision: 18, scale: 6, nullable: false),
                    Width = table.Column<double>(type: "double precision", precision: 18, scale: 6, nullable: false),
                    Height = table.Column<double>(type: "double precision", precision: 18, scale: 6, nullable: true),
                    DesignId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoSlides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoSlides_Designs_DesignId",
                        column: x => x.DesignId,
                        principalTable: "Designs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PresentationIsroilovs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TitleId = table.Column<int>(type: "integer", nullable: false),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    PlanId = table.Column<int>(type: "integer", nullable: false),
                    DesignId = table.Column<int>(type: "integer", nullable: false),
                    WithPhoto = table.Column<bool>(type: "boolean", nullable: false),
                    PageCount = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    FilePath = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentationIsroilovs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresentationIsroilovs_Designs_DesignId",
                        column: x => x.DesignId,
                        principalTable: "Designs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PresentationIsroilovs_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PresentationIsroilovs_TextSlides_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "TextSlides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PresentationIsroilovs_TextSlides_TitleId",
                        column: x => x.TitleId,
                        principalTable: "TextSlides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PresentationPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhotoId = table.Column<int>(type: "integer", nullable: true),
                    BackgroundPhotoId = table.Column<int>(type: "integer", nullable: true),
                    PresentationIsroilovId = table.Column<int>(type: "integer", nullable: false),
                    WithPhoto = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentationPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresentationPages_PhotoSlides_BackgroundPhotoId",
                        column: x => x.BackgroundPhotoId,
                        principalTable: "PhotoSlides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PresentationPages_PhotoSlides_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "PhotoSlides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PresentationPages_PresentationIsroilovs_PresentationIsroilo~",
                        column: x => x.PresentationIsroilovId,
                        principalTable: "PresentationIsroilovs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PresentationPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PresentationPageId = table.Column<int>(type: "integer", nullable: false),
                    TitleId = table.Column<int>(type: "integer", nullable: true),
                    TextId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentationPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresentationPosts_PresentationPages_PresentationPageId",
                        column: x => x.PresentationPageId,
                        principalTable: "PresentationPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PresentationPosts_TextSlides_TextId",
                        column: x => x.TextId,
                        principalTable: "TextSlides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PresentationPosts_TextSlides_TitleId",
                        column: x => x.TitleId,
                        principalTable: "TextSlides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminActions_ActionType",
                table: "AdminActions",
                column: "ActionType");

            migrationBuilder.CreateIndex(
                name: "IX_AdminActions_AdminId",
                table: "AdminActions",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminActions_AdminId_ActionType",
                table: "AdminActions",
                columns: new[] { "AdminId", "ActionType" });

            migrationBuilder.CreateIndex(
                name: "IX_AdminActions_CreatedAt",
                table: "AdminActions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AdminActions_TargetUserId",
                table: "AdminActions",
                column: "TargetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminActions_TargetUserId_ActionType",
                table: "AdminActions",
                columns: new[] { "TargetUserId", "ActionType" });

            migrationBuilder.CreateIndex(
                name: "IX_Designs_CreatedAt",
                table: "Designs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Designs_CreatedById",
                table: "Designs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Designs_Title",
                table: "Designs",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_Status",
                table: "Notifications",
                columns: new[] { "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenaiKeys_CreatedAt",
                table: "OpenaiKeys",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_OpenaiKeys_Key",
                table: "OpenaiKeys",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenaiKeys_UpdatedAt",
                table: "OpenaiKeys",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_OpenaiKeys_UseCount",
                table: "OpenaiKeys",
                column: "UseCount");

            migrationBuilder.CreateIndex(
                name: "IX_OpenaiKeys_UseCount_CreatedAt",
                table: "OpenaiKeys",
                columns: new[] { "UseCount", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreatedAt",
                table: "Payments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentStatus",
                table: "Payments",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ProcessedByAdminId",
                table: "Payments",
                column: "ProcessedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SenderId_PaymentStatus",
                table: "Payments",
                columns: new[] { "SenderId", "PaymentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_PhotoSlides_ContentType",
                table: "PhotoSlides",
                column: "ContentType");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoSlides_CreatedAt",
                table: "PhotoSlides",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoSlides_DesignId",
                table: "PhotoSlides",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoSlides_FileSize",
                table: "PhotoSlides",
                column: "FileSize");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoSlides_Left_Top",
                table: "PhotoSlides",
                columns: new[] { "Left", "Top" });

            migrationBuilder.CreateIndex(
                name: "IX_PhotoSlides_OriginalFileName",
                table: "PhotoSlides",
                column: "OriginalFileName");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoSlides_PhotoPath",
                table: "PhotoSlides",
                column: "PhotoPath");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoSlides_UpdatedAt",
                table: "PhotoSlides",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_PlansId",
                table: "Plans",
                column: "PlansId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_PlanTextId",
                table: "Plans",
                column: "PlanTextId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationIsroilovs_AuthorId",
                table: "PresentationIsroilovs",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationIsroilovs_CreatedAt",
                table: "PresentationIsroilovs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationIsroilovs_DesignId",
                table: "PresentationIsroilovs",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationIsroilovs_IsActive",
                table: "PresentationIsroilovs",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationIsroilovs_PlanId",
                table: "PresentationIsroilovs",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationIsroilovs_TitleId",
                table: "PresentationIsroilovs",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationPages_BackgroundPhotoId",
                table: "PresentationPages",
                column: "BackgroundPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationPages_CreatedAt",
                table: "PresentationPages",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationPages_PhotoId",
                table: "PresentationPages",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationPages_PresentationIsroilovId",
                table: "PresentationPages",
                column: "PresentationIsroilovId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationPosts_CreatedAt",
                table: "PresentationPosts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationPosts_PresentationPageId",
                table: "PresentationPosts",
                column: "PresentationPageId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationPosts_TextId",
                table: "PresentationPosts",
                column: "TextId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationPosts_TitleId",
                table: "PresentationPosts",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_TextSlides_ColorHex",
                table: "TextSlides",
                column: "ColorHex");

            migrationBuilder.CreateIndex(
                name: "IX_TextSlides_CreatedAt",
                table: "TextSlides",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TextSlides_Font",
                table: "TextSlides",
                column: "Font");

            migrationBuilder.CreateIndex(
                name: "IX_TextSlides_IsBold_IsItalic",
                table: "TextSlides",
                columns: new[] { "IsBold", "IsItalic" });

            migrationBuilder.CreateIndex(
                name: "IX_TextSlides_Left_Top",
                table: "TextSlides",
                columns: new[] { "Left", "Top" });

            migrationBuilder.CreateIndex(
                name: "IX_TextSlides_Size",
                table: "TextSlides",
                column: "Size");

            migrationBuilder.CreateIndex(
                name: "IX_TextSlides_Text_Left_Top",
                table: "TextSlides",
                columns: new[] { "Text", "Left", "Top" });

            migrationBuilder.CreateIndex(
                name: "IX_TextSlides_UpdatedAt",
                table: "TextSlides",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TelegramId",
                table: "Users",
                column: "TelegramId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VerificationCodes_Code",
                table: "VerificationCodes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationCodes_UserId_IsUsed",
                table: "VerificationCodes",
                columns: new[] { "UserId", "IsUsed" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminActions");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OpenaiKeys");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PresentationPosts");

            migrationBuilder.DropTable(
                name: "VerificationCodes");

            migrationBuilder.DropTable(
                name: "PresentationPages");

            migrationBuilder.DropTable(
                name: "PhotoSlides");

            migrationBuilder.DropTable(
                name: "PresentationIsroilovs");

            migrationBuilder.DropTable(
                name: "Designs");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TextSlides");
        }
    }
}
