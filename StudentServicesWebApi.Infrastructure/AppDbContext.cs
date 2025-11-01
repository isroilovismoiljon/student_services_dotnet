using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
    public DbSet<PresentationIsroilov> PresentationIsroilovs => Set<PresentationIsroilov>();
    public DbSet<PresentationPage> PresentationPages => Set<PresentationPage>();
    public DbSet<PresentationPost> PresentationPosts => Set<PresentationPost>();
    public DbSet<Design> Designs => Set<Design>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<OpenaiKey> OpenaiKeys => Set<OpenaiKey>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username);
                
            entity.HasIndex(u => u.TelegramId)
                .IsUnique();
        });
        modelBuilder.Entity<Notification>()
            .HasIndex(n => new { n.UserId, n.Status });
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<VerificationCode>()
            .HasIndex(vc => vc.Code);
        modelBuilder.Entity<VerificationCode>()
            .HasIndex(vc => new { vc.UserId, vc.IsUsed });
        modelBuilder.Entity<VerificationCode>()
            .HasOne(vc => vc.User)
            .WithMany()
            .HasForeignKey(vc => vc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(p => p.Photo)
                .IsRequired()
                .HasMaxLength(2000); 
            entity.Property(p => p.RequestedAmount)
                .IsRequired()
                .HasPrecision(18, 2); 
            entity.Property(p => p.ApprovedAmount)
                .HasPrecision(18, 2); 
            entity.Property(p => p.Description)
                .HasMaxLength(500);
            entity.Property(p => p.RejectReason)
                .HasMaxLength(1000);
            entity.Property(p => p.AdminNotes)
                .HasMaxLength(1000);
            entity.HasIndex(p => p.PaymentStatus);
            entity.HasIndex(p => new { p.SenderId, p.PaymentStatus });
            entity.HasIndex(p => p.ProcessedByAdminId);
            entity.HasIndex(p => p.CreatedAt);
            entity.HasOne(p => p.Sender)
                .WithMany()
                .HasForeignKey(p => p.SenderId)
                .OnDelete(DeleteBehavior.Restrict); 
            entity.HasOne(p => p.ProcessedByAdmin)
                .WithMany()
                .HasForeignKey(p => p.ProcessedByAdminId)
                .OnDelete(DeleteBehavior.Restrict) 
                .IsRequired(false);
        });
        modelBuilder.Entity<AdminAction>(entity =>
        {
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
                .HasPrecision(18, 2); 
            entity.HasIndex(a => a.AdminId);
            entity.HasIndex(a => a.TargetUserId);
            entity.HasIndex(a => a.ActionType);
            entity.HasIndex(a => a.CreatedAt);
            entity.HasIndex(a => new { a.AdminId, a.ActionType });
            entity.HasIndex(a => new { a.TargetUserId, a.ActionType });
            entity.HasOne(a => a.Admin)
                .WithMany()
                .HasForeignKey(a => a.AdminId)
                .OnDelete(DeleteBehavior.Restrict); 
            entity.HasOne(a => a.TargetUser)
                .WithMany()
                .HasForeignKey(a => a.TargetUserId)
                .OnDelete(DeleteBehavior.Restrict); 
        });
        modelBuilder.Entity<TextSlide>(entity =>
        {
            entity.Property(ts => ts.Text)
                .IsRequired()
                .HasMaxLength(5000); 
            entity.Property(ts => ts.Font)
                .IsRequired()
                .HasMaxLength(100); 
            entity.Property(ts => ts.ColorHex)
                .IsRequired()
                .HasMaxLength(7) 
;
            entity.Property(ts => ts.Size)
                .IsRequired();
            entity.Property(ts => ts.Left)
                .IsRequired()
                .HasPrecision(18, 6); 
            entity.Property(ts => ts.Top)
                .IsRequired()
                .HasPrecision(18, 6);
            entity.Property(ts => ts.Width)
                .IsRequired()
                .HasPrecision(18, 6);
            entity.Property(ts => ts.Height)
                .HasPrecision(18, 6); 
            entity.HasIndex(ts => ts.Font);
            entity.HasIndex(ts => ts.ColorHex);
            entity.HasIndex(ts => ts.Size);
            entity.HasIndex(ts => new { ts.IsBold, ts.IsItalic });
            entity.HasIndex(ts => new { ts.Left, ts.Top });
            entity.HasIndex(ts => ts.CreatedAt);
            entity.HasIndex(ts => ts.UpdatedAt);
            entity.HasIndex(ts => new { ts.Text, ts.Left, ts.Top })
;
        });
        modelBuilder.Entity<PhotoSlide>(entity =>
        {
            entity.Property(ps => ps.PhotoPath)
                .IsRequired()
                .HasMaxLength(2000); 
            entity.Property(ps => ps.OriginalFileName)
                .IsRequired()
                .HasMaxLength(500); 
            entity.Property(ps => ps.ContentType)
                .IsRequired()
                .HasMaxLength(100); 
            entity.Property(ps => ps.FileSize)
                .IsRequired();
            entity.Property(ps => ps.Left)
                .IsRequired()
                .HasPrecision(18, 6); 
            entity.Property(ps => ps.Top)
                .IsRequired()
                .HasPrecision(18, 6);
            entity.Property(ps => ps.Width)
                .IsRequired()
                .HasPrecision(18, 6);
            entity.Property(ps => ps.Height)
                .HasPrecision(18, 6); 
            entity.HasIndex(ps => ps.PhotoPath);
            entity.HasIndex(ps => ps.OriginalFileName);
            entity.HasIndex(ps => ps.ContentType);
            entity.HasIndex(ps => ps.FileSize);
            entity.HasIndex(ps => new { ps.Left, ps.Top });
            entity.HasIndex(ps => ps.CreatedAt);
            entity.HasIndex(ps => ps.UpdatedAt);
            entity.HasIndex(ps => new { ps.Left, ps.Top })
;
        });
        modelBuilder.Entity<PresentationIsroilov>(entity =>
        {
            entity.Property(p => p.FilePath)
                .HasMaxLength(2000);
            entity.HasIndex(p => p.TitleId);
            entity.HasIndex(p => p.AuthorId);
            entity.HasIndex(p => p.IsActive);
            entity.HasIndex(p => p.CreatedAt);
            entity.HasOne(p => p.Title)
                .WithMany()
                .HasForeignKey(p => p.TitleId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(p => p.Author)
                .WithMany()
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<PresentationPage>(entity =>
        {
            entity.HasIndex(pp => pp.PresentationIsroilovId);
            entity.HasIndex(pp => pp.CreatedAt);
            entity.HasOne(pp => pp.PresentationIsroilov)
                .WithMany(p => p.PresentationPages)
                .HasForeignKey(pp => pp.PresentationIsroilovId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(pp => pp.Photo)
                .WithMany()
                .HasForeignKey(pp => pp.PhotoId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
            entity.HasOne(pp => pp.BackgroundPhoto)
                .WithMany()
                .HasForeignKey(pp => pp.BackgroundPhotoId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        });
        modelBuilder.Entity<PresentationPost>(entity =>
        {
            entity.HasIndex(pp => pp.PresentationPageId);
            entity.HasIndex(pp => pp.TitleId);
            entity.HasIndex(pp => pp.TextId);
            entity.HasIndex(pp => pp.CreatedAt);
            entity.HasOne(pp => pp.PresentationPage)
                .WithMany(p => p.PresentationPosts)
                .HasForeignKey(pp => pp.PresentationPageId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(pp => pp.Title)
                .WithMany()
                .HasForeignKey(pp => pp.TitleId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
            entity.HasOne(pp => pp.Text)
                .WithMany()
                .HasForeignKey(pp => pp.TextId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Design>(entity =>
        {
            entity.Property(d => d.Title)
                .IsRequired()
                .HasMaxLength(200);
            entity.HasIndex(d => d.Title);
            entity.HasIndex(d => d.CreatedAt);
            entity.HasOne(d => d.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(d => d.Photos)
                .WithOne()
                .HasForeignKey("DesignId")
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<OpenaiKey>(entity =>
        {
            entity.Property(ok => ok.Key)
                .IsRequired()
                .HasMaxLength(256); 
            entity.Property(ok => ok.UseCount)
                .IsRequired();
            entity.HasIndex(ok => ok.Key)
                .IsUnique();
            entity.HasIndex(ok => ok.UseCount);
            entity.HasIndex(ok => ok.CreatedAt);
            entity.HasIndex(ok => ok.UpdatedAt);
            entity.HasIndex(ok => new { ok.UseCount, ok.CreatedAt });
        });
    }
}
