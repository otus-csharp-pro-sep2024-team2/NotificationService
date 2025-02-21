using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Configurations;

public class NotificationRecepientLinkCongfiguration : IEntityTypeConfiguration<NotificationRecepientLink>
    {
        public void Configure(EntityTypeBuilder<NotificationRecepientLink> builder)
        {
            // Указываем таблицу, в которую будет отображаться сущность
            builder.ToTable("NotificationRecepientLinks");

            // Настройка составного первичного ключа
            builder.HasKey(nrl => new { nrl.NotificationId, nrl.RecepientId });

            // Настройка связи с Notification
            builder.HasOne<Notification>()
                .WithMany(n => n.Links)
                .HasForeignKey(nrl => nrl.NotificationId);

            // Настройка связи с Recepient
            builder.HasOne<Recipient>()
                .WithMany(r => r.Links)
                .HasForeignKey(nrl => nrl.RecepientId);
        }
    }
