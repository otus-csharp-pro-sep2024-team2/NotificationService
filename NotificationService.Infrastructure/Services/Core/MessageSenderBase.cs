using Microsoft.Extensions.Logging;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Services.Interfaces;

namespace NotificationService.Infrastructure.Services.Core;

public abstract class MessageSenderBase(ILogger logger) : IMessageSender
{
    public abstract Task SendAsync(Message message);
}