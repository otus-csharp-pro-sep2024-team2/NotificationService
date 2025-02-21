using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Recipient> Recepients { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationRecepientLink> NotificationRecepientLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.HasDefaultSchema("lessonsrv");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);
    }
    
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Notification>(entity =>
    //     {
    //         entity.HasKey(n => n.Id);
    //         entity.Property(n => n.UserId)
    //             .IsRequired()
    //             .HasMaxLength(50);
    //         entity.Property(n => n.Message)
    //             .IsRequired();
    //         entity.Property(n => n.CreatedAt)
    //             .IsRequired();
    //     });
    // }
}