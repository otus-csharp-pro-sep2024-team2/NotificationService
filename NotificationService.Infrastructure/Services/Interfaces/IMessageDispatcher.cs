using NotificationService.Domain;

namespace NotificationService.Infrastructure.Services.Interfaces;

public interface IMessageDispatcher
{
    Task SendAllAsync(NotificationDto notification);
  
}