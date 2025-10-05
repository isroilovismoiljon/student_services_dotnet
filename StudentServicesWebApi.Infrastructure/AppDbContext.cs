using Microsoft.EntityFrameworkCore;
using StudentServicesWebApi.Domain.Models;

namespace StudentServicesWebApi.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<VerificationCode> VerificationCodes => Set<VerificationCode>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<AdminAction> AdminActions => Set<AdminAction>();
    public DbSet<TextSlide> TextSlides => Set<TextSlide>();
    public DbSet<PhotoSlide> PhotoSlides => Set<PhotoSlide>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
            
        modelBuilder.Entity<User>()
            .HasIndex(u => u.TelegramId)
            .IsUnique(); // TelegramId unique constraint

        // Notification
        modelBuilder.Entity<Notification>()
            .HasIndex(n => new { n.UserId, n.Status });

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // VerificationCode
        modelBuilder.Entity<VerificationCode>()
            .HasIndex(vc => vc.Code);

        modelBuilder.Entity<VerificationCode>()
            .HasIndex(vc => new { vc.UserId, vc.IsUsed });

        modelBuilder.Entity<VerificationCode>()
            .HasOne(vc => vc.User)
            .WithMany()
            .HasForeignKey(vc => vc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            // Required fields
            entity.Property(p => p.Photo)
                .IsRequired()
                .HasMaxLength(2000); // Allow for URLs or file paths
            
            entity.Property(p => p.RequestedAmount)
                .IsRequired()
                .HasPrecision(18, 2); // Decimal precision for currency
            
            entity.Property(p => p.ApprovedAmount)
                .HasPrecision(18, 2); // Optional decimal for approved amount
            
            // String length constraints
            entity.Property(p => p.Description)
                .HasMaxLength(500);
            
            entity.Property(p => p.RejectReason)
                .HasMaxLength(1000);
            
            entity.Property(p => p.AdminNotes)
                .HasMaxLength(1000);
            
            // Indexes for performance
            entity.HasIndex(p => p.PaymentStatus);
            entity.HasIndex(p => new { p.SenderId, p.PaymentStatus });
            entity.HasIndex(p => p.ProcessedByAdminId);
            entity.HasIndex(p => p.CreatedAt);
            
            // Foreign key relationships
            entity.HasOne(p => p.Sender)
                .WithMany()
                .HasForeignKey(p => p.SenderId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            
            
            entity.HasOne(p => p.ProcessedByAdmin)
                .WithMany()
                .HasForeignKey(p => p.ProcessedByAdminId)
                .OnDelete(DeleteBehavior.Restrict) // Prevent cascade delete
                .IsRequired(false);
        });

        // AdminAction
        modelBuilder.Entity<AdminAction>(entity =>
        {
            // String length constraints
            entity.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(1000);
            
            entity.Property(a => a.PreviousValue)
                .HasMaxLength(100);
            
            entity.Property(a => a.NewValue)
                .HasMaxLength(100);
            
            entity.Property(a => a.Reason)
                .HasMaxLength(500);
            
            entity.Property(a => a.IpAddress)
                .HasMaxLength(50);
            
            entity.Property(a => a.Amount)
                .HasPrecision(18, 2); // Decimal precision for currency
            
            // Indexes for performance
            entity.HasIndex(a => a.AdminId);
            entity.HasIndex(a => a.TargetUserId);
            entity.HasIndex(a => a.ActionType);
            entity.HasIndex(a => a.CreatedAt);
            entity.HasIndex(a => new { a.AdminId, a.ActionType });
            entity.HasIndex(a => new { a.TargetUserId, a.ActionType });
            
            // Foreign key relationships
            entity.HasOne(a => a.Admin)
                .WithMany()
                .HasForeignKey(a => a.AdminId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            
            entity.HasOne(a => a.TargetUser)
                .WithMany()
                .HasForeignKey(a => a.TargetUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        });

        // TextSlide
        modelBuilder.Entity<TextSlide>(entity =>
        {
            // Required fields
            entity.Property(ts => ts.Text)
                .IsRequired()
                .HasMaxLength(5000); // Allow for large text content
            
            entity.Property(ts => ts.Font)
                .IsRequired()
                .HasMaxLength(100); // Font name constraint
            
            entity.Property(ts => ts.ColorHex)
                .IsRequired()
                .HasMaxLength(7) // #FFFFFF format
;
            
            // Numeric constraints
            entity.Property(ts => ts.Size)
                .IsRequired();
            
            entity.Property(ts => ts.Left)
                .IsRequired()
                .HasPrecision(18, 6); // High precision for positioning
            
            entity.Property(ts => ts.Top)
                .IsRequired()
                .HasPrecision(18, 6);
            
            entity.Property(ts => ts.Width)
                .IsRequired()
                .HasPrecision(18, 6);
            
            entity.Property(ts => ts.Height)
                .HasPrecision(18, 6); // Optional, so nullable
            
            // Indexes for performance
            entity.HasIndex(ts => ts.Font);
            entity.HasIndex(ts => ts.ColorHex);
            entity.HasIndex(ts => ts.Size);
            entity.HasIndex(ts => new { ts.IsBold, ts.IsItalic });
            entity.HasIndex(ts => new { ts.Left, ts.Top });
            entity.HasIndex(ts => ts.CreatedAt);
            entity.HasIndex(ts => ts.UpdatedAt);
            
            // Full text search index for text content (PostgreSQL specific)
            // entity.HasIndex(ts => ts.Text).HasMethod("gin").IsTsVectorExpressionIndex("english");
            
            // Composite index for duplicate detection
            entity.HasIndex(ts => new { ts.Text, ts.Left, ts.Top })
;
        });

        // PhotoSlide
        modelBuilder.Entity<PhotoSlide>(entity =>
        {
            // Required fields
            entity.Property(ps => ps.PhotoPath)
                .IsRequired()
                .HasMaxLength(2000); // Allow for long file paths
            
            entity.Property(ps => ps.OriginalFileName)
                .IsRequired()
                .HasMaxLength(500); // Filename constraint
            
            entity.Property(ps => ps.ContentType)
                .IsRequired()
                .HasMaxLength(100); // MIME type constraint
            
            // Numeric constraints
            entity.Property(ps => ps.FileSize)
                .IsRequired();
            
            entity.Property(ps => ps.Left)
                .IsRequired()
                .HasPrecision(18, 6); // High precision for positioning
            
            entity.Property(ps => ps.Top)
                .IsRequired()
                .HasPrecision(18, 6);
            
            entity.Property(ps => ps.Width)
                .IsRequired()
                .HasPrecision(18, 6);
            
            entity.Property(ps => ps.Height)
                .HasPrecision(18, 6); // Optional, so nullable
            
            // Indexes for performance
            entity.HasIndex(ps => ps.PhotoPath);
            entity.HasIndex(ps => ps.OriginalFileName);
            entity.HasIndex(ps => ps.ContentType);
            entity.HasIndex(ps => ps.FileSize);
            entity.HasIndex(ps => new { ps.Left, ps.Top });
            entity.HasIndex(ps => ps.CreatedAt);
            entity.HasIndex(ps => ps.UpdatedAt);
            
            // Composite index for duplicate position detection
            entity.HasIndex(ps => new { ps.Left, ps.Top })
;
        });
    }
}
