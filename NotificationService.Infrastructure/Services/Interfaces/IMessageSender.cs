using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Services.Interfaces;

public interface IMessageSender
{
    Task SendAsync(Message message);
}