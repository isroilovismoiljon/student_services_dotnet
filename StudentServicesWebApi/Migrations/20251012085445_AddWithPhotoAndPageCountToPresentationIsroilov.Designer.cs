using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StudentServicesWebApi.Infrastructure;
#nullable disable
namespace StudentServicesWebApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20251012085445_AddWithPhotoAndPageCountToPresentationIsroilov")]
    partial class AddWithPhotoAndPageCountToPresentationIsroilov
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.AdminAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");
                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    b.Property<int>("ActionType")
                        .HasColumnType("integer");
                    b.Property<int>("AdminId")
                        .HasColumnType("integer");
                    b.Property<decimal?>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");
                    b.Property<string>("IpAddress")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");
                    b.Property<string>("NewValue")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");
                    b.Property<bool>("NotificationSent")
                        .HasColumnType("boolean");
                    b.Property<string>("PreviousValue")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");
                    b.Property<string>("Reason")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");
                    b.Property<int>("TargetUserId")
                        .HasColumnType("integer");
                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.HasKey("Id");
                    b.HasIndex("ActionType");
                    b.HasIndex("AdminId");
                    b.HasIndex("CreatedAt");
                    b.HasIndex("TargetUserId");
                    b.HasIndex("AdminId", "ActionType");
                    b.HasIndex("TargetUserId", "ActionType");
                    b.ToTable("AdminActions");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.Design", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");
                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<int>("CreatedById")
                        .HasColumnType("integer");
                    b.Property<bool>("IsValid")
                        .HasColumnType("boolean");
                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");
                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.HasKey("Id");
                    b.HasIndex("CreatedAt");
                    b.HasIndex("CreatedById");
                    b.HasIndex("Title");
                    b.ToTable("Designs");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");
                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    b.Property<string>("ActionUrl")
                        .HasColumnType("text");
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<string>("IconUrl")
                        .HasColumnType("text");
                    b.Property<bool>("IsGlobal")
                        .HasColumnType("boolean");
                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");
                    b.Property<string>("Metadata")
                        .HasColumnType("text");
                    b.Property<DateTime?>("ReadAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<int>("Status")
                        .HasColumnType("integer");
                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");
                    b.Property<int>("Type")
                        .HasColumnType("integer");
                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<int>("UserId")
                        .HasColumnType("integer");
                    b.HasKey("Id");
                    b.HasIndex("UserId", "Status");
                    b.ToTable("Notifications");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.OpenaiKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");
                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");
                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<int>("UseCount")
                        .HasColumnType("integer");
                    b.HasKey("Id");
                    b.HasIndex("CreatedAt");
                    b.HasIndex("Key")
                        .IsUnique();
                    b.HasIndex("UpdatedAt");
                    b.HasIndex("UseCount");
                    b.HasIndex("UseCount", "CreatedAt");
                    b.ToTable("OpenaiKeys");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");
                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    b.Property<string>("AdminNotes")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");
                    b.Property<decimal?>("ApprovedAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");
                    b.Property<int>("PaymentStatus")
                        .HasColumnType("integer");
                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");
                    b.Property<DateTime?>("ProcessedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<int?>("ProcessedByAdminId")
                        .HasColumnType("integer");
                    b.Property<string>("RejectReason")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");
                    b.Property<decimal>("RequestedAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");
                    b.Property<int>("SenderId")
                        .HasColumnType("integer");
                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.HasKey("Id");
                    b.HasIndex("CreatedAt");
                    b.HasIndex("PaymentStatus");
                    b.HasIndex("ProcessedByAdminId");
                    b.HasIndex("SenderId", "PaymentStatus");
                    b.ToTable("Payments");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.PhotoSlide", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");
                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<int?>("DesignId")
                        .HasColumnType("integer");
                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");
                    b.Property<double?>("Height")
                        .HasPrecision(18, 6)
                        .HasColumnType("double precision");
                    b.Property<double>("Left")
                        .HasPrecision(18, 6)
                        .HasColumnType("double precision");
                    b.Property<string>("OriginalFileName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");
                    b.Property<string>("PhotoPath")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");
                    b.Property<double>("Top")
                        .HasPrecision(18, 6)
                        .HasColumnType("double precision");
                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<double>("Width")
                        .HasPrecision(18, 6)
                        .HasColumnType("double precision");
                    b.HasKey("Id");
                    b.HasIndex("ContentType");
                    b.HasIndex("CreatedAt");
                    b.HasIndex("DesignId");
                    b.HasIndex("FileSize");
                    b.HasIndex("OriginalFileName");
                    b.HasIndex("PhotoPath");
                    b.HasIndex("UpdatedAt");
                    b.HasIndex("Left", "Top");
                    b.ToTable("PhotoSlides");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");
                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<int>("PlanTextId")
                        .HasColumnType("integer");
                    b.Property<int>("PlansId")
                        .HasColumnType("integer");
                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.HasKey("Id");
                    b.HasIndex("PlanTextId");
                    b.HasIndex("PlansId");
                    b.ToTable("Plans");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.PresentationIsroilov", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");
                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<int>("DesignId")
                        .HasColumnType("integer");
                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");
                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");
                    b.Property<int>("PageCount")
                        .HasColumnType("integer");
                    b.Property<int>("PlanId")
                        .HasColumnType("integer");
                    b.Property<int>("TitleId")
                        .HasColumnType("integer");
                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<bool>("WithPhoto")
                        .HasColumnType("boolean");
                    b.HasKey("Id");
                    b.HasIndex("AuthorId");
                    b.HasIndex("CreatedAt");
                    b.HasIndex("DesignId");
                    b.HasIndex("IsActive");
                    b.HasIndex("PlanId");
                    b.HasIndex("TitleId");
                    b.ToTable("PresentationIsroilovs");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.PresentationPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");
                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    b.Property<int?>("BackgroundPhotoId")
                        .HasColumnType("integer");
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<int?>("PhotoId")
                        .HasColumnType("integer");
                    b.Property<int>("PresentationIsroilovId")
                        .HasColumnType("integer");
                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.HasKey("Id");
                    b.HasIndex("BackgroundPhotoId");
                    b.HasIndex("CreatedAt");
                    b.HasIndex("PhotoId");
                    b.HasIndex("PresentationIsroilovId");
                    b.ToTable("PresentationPages");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.PresentationPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");
                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<int>("PresentationPageId")
                        .HasColumnType("integer");
                    b.Property<int>("TextId")
                        .HasColumnType("integer");
                    b.Property<int?>("TitleId")
                        .HasColumnType("integer");
                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.HasKey("Id");
                    b.HasIndex("CreatedAt");
                    b.HasIndex("PresentationPageId");
                    b.HasIndex("TextId");
                    b.HasIndex("TitleId");
                    b.ToTable("PresentationPosts");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.TextSlide", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");
                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    b.Property<string>("ColorHex")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)");
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<string>("Font")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");
                    b.Property<double?>("Height")
                        .HasPrecision(18, 6)
                        .HasColumnType("double precision");
                    b.Property<int>("Horizontal")
                        .HasColumnType("integer");
                    b.Property<bool>("IsBold")
                        .HasColumnType("boolean");
                    b.Property<bool>("IsItalic")
                        .HasColumnType("boolean");
                    b.Property<double>("Left")
                        .HasPrecision(18, 6)
                        .HasColumnType("double precision");
                    b.Property<int>("Size")
                        .HasColumnType("integer");
                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("character varying(5000)");
                    b.Property<double>("Top")
                        .HasPrecision(18, 6)
                        .HasColumnType("double precision");
                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<int>("Vertical")
                        .HasColumnType("integer");
                    b.Property<double>("Width")
                        .HasPrecision(18, 6)
                        .HasColumnType("double precision");
                    b.HasKey("Id");
                    b.HasIndex("ColorHex");
                    b.HasIndex("CreatedAt");
                    b.HasIndex("Font");
                    b.HasIndex("Size");
                    b.HasIndex("UpdatedAt");
                    b.HasIndex("IsBold", "IsItalic");
                    b.HasIndex("Left", "Top");
                    b.HasIndex("Text", "Left", "Top");
                    b.ToTable("TextSlides");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");
                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    b.Property<int>("Balance")
                        .HasColumnType("integer");
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");
                    b.Property<bool>("IsVerified")
                        .HasColumnType("boolean");
                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");
                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");
                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");
                    b.Property<string>("Photo")
                        .HasColumnType("text");
                    b.Property<int?>("ReferralId")
                        .HasColumnType("integer");
                    b.Property<string>("TelegramId")
                        .HasColumnType("text");
                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<int>("UserRole")
                        .HasColumnType("integer");
                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");
                    b.HasKey("Id");
                    b.HasIndex("TelegramId")
                        .IsUnique();
                    b.HasIndex("Username")
                        .IsUnique();
                    b.ToTable("Users");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.VerificationCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");
                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");
                    b.Property<int>("CodeType")
                        .HasColumnType("integer");
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean");
                    b.Property<string>("TelegramDeepLink")
                        .HasColumnType("text");
                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");
                    b.Property<int>("UserId")
                        .HasColumnType("integer");
                    b.HasKey("Id");
                    b.HasIndex("Code");
                    b.HasIndex("UserId", "IsUsed");
                    b.ToTable("VerificationCodes");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.AdminAction", b =>
                {
                    b.HasOne("StudentServicesWebApi.Domain.Models.User", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                    b.HasOne("StudentServicesWebApi.Domain.Models.User", "TargetUser")
                        .WithMany()
                        .HasForeignKey("TargetUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                    b.Navigation("Admin");
                    b.Navigation("TargetUser");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.Design", b =>
                {
                    b.HasOne("StudentServicesWebApi.Domain.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                    b.Navigation("CreatedBy");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.Notification", b =>
                {
                    b.HasOne("StudentServicesWebApi.Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.Navigation("User");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.Payment", b =>
                {
                    b.HasOne("StudentServicesWebApi.Domain.Models.User", "ProcessedByAdmin")
                        .WithMany()
                        .HasForeignKey("ProcessedByAdminId")
                        .OnDelete(DeleteBehavior.Restrict);
                    b.HasOne("StudentServicesWebApi.Domain.Models.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                    b.Navigation("ProcessedByAdmin");
                    b.Navigation("Sender");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.PhotoSlide", b =>
                {
                    b.HasOne("StudentServicesWebApi.Domain.Models.Design", null)
                        .WithMany("Photos")
                        .HasForeignKey("DesignId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.Plan", b =>
                {
                    b.HasOne("StudentServicesWebApi.Domain.Models.TextSlide", "PlanText")
                        .WithMany()
                        .HasForeignKey("PlanTextId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.HasOne("StudentServicesWebApi.Domain.Models.TextSlide", "Plans")
                        .WithMany()
                        .HasForeignKey("PlansId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.Navigation("PlanText");
                    b.Navigation("Plans");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.PresentationIsroilov", b =>
                {
                    b.HasOne("StudentServicesWebApi.Domain.Models.TextSlide", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                    b.HasOne("StudentServicesWebApi.Domain.Models.Design", "Design")
                        .WithMany()
                        .HasForeignKey("DesignId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                    b.HasOne("StudentServicesWebApi.Domain.Models.Plan", "Plan")
                        .WithMany()
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                    b.HasOne("StudentServicesWebApi.Domain.Models.TextSlide", "Title")
                        .WithMany()
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                    b.Navigation("Author");
                    b.Navigation("Design");
                    b.Navigation("Plan");
                    b.Navigation("Title");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.PresentationPage", b =>
                {
                    b.HasOne("StudentServicesWebApi.Domain.Models.PhotoSlide", "BackgroundPhoto")
                        .WithMany()
                        .HasForeignKey("BackgroundPhotoId")
                        .OnDelete(DeleteBehavior.SetNull);
                    b.HasOne("StudentServicesWebApi.Domain.Models.PhotoSlide", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.SetNull);
                    b.HasOne("StudentServicesWebApi.Domain.Models.PresentationIsroilov", "PresentationIsroilov")
                        .WithMany("PresentationPages")
                        .HasForeignKey("PresentationIsroilovId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.Navigation("BackgroundPhoto");
                    b.Navigation("Photo");
                    b.Navigation("PresentationIsroilov");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.PresentationPost", b =>
                {
                    b.HasOne("StudentServicesWebApi.Domain.Models.PresentationPage", "PresentationPage")
                        .WithMany("PresentationPosts")
                        .HasForeignKey("PresentationPageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.HasOne("StudentServicesWebApi.Domain.Models.TextSlide", "Text")
                        .WithMany()
                        .HasForeignKey("TextId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                    b.HasOne("StudentServicesWebApi.Domain.Models.TextSlide", "Title")
                        .WithMany()
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.SetNull);
                    b.Navigation("PresentationPage");
                    b.Navigation("Text");
                    b.Navigation("Title");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.VerificationCode", b =>
                {
                    b.HasOne("StudentServicesWebApi.Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.Navigation("User");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.Design", b =>
                {
                    b.Navigation("Photos");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.PresentationIsroilov", b =>
                {
                    b.Navigation("PresentationPages");
                });
            modelBuilder.Entity("StudentServicesWebApi.Domain.Models.PresentationPage", b =>
                {
                    b.Navigation("PresentationPosts");
                });
#pragma warning restore 612, 618
        }
    }
}
