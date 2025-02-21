using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        // Указываем таблицу, в которую будет отображаться сущность
        builder.ToTable("Notifications");

        // Настройка первичного ключа
        builder.HasKey(n => n.Id);

        // Настройка свойства Type
        builder.Property(n => n.Type)
            .IsRequired();

        // Настройка свойства Message
        builder.Property(n => n.Message)
            .HasMaxLength(1000) // Ограничение длины строки
            .IsRequired();

        // Настройка свойства CreatedAt
        builder.Property(n => n.CreatedAt)
            .IsRequired();

        // Настройка связи с NotificationRecepientLink
        builder.HasMany(n => n.Links)
            .WithOne()
            .HasForeignKey(nrl => nrl.NotificationId)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление
    }
}