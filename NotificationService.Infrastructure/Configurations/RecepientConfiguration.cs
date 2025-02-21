using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Configurations;

public class RecepientConfiguration : IEntityTypeConfiguration<Recipient>
{
    public void Configure(EntityTypeBuilder<Recipient> builder)
    {
// Указываем таблицу, в которую будет отображаться сущность
        builder.ToTable("Recepients");

        // Настройка первичного ключа
        builder.HasKey(r => r.Id);

        // Настройка свойства RecepientId
        builder.Property(r => r.RecipientId)
            .IsRequired();

        // Настройка свойства ContactType
        builder.Property(r => r.ContactType)
            .IsRequired();

        // Настройка свойства Address
        builder.Property(r => r.Address)
            .HasMaxLength(255) // Ограничение длины строки
            .IsRequired();

        // Настройка связи с NotificationRecepientLink
        builder.HasMany(r => r.Links)
            .WithOne()
            .HasForeignKey(nrl => nrl.RecepientId)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление
    }    
}