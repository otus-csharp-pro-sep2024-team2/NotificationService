using NotificationService.Domain;

namespace NotificationService.Infrastructure.Services.Interfaces;

public interface INotificationConverter
{
    NotificationDto Convert(string message);
}