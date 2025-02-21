using Microsoft.Extensions.Logging;
using NotificationService.Domain;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Services.Interfaces;

namespace NotificationService.Infrastructure.Implemetations;

public class MessageDispatcher(
    ILogger<MessageDispatcher> logger,
    IMessageSenderCollection senderCollection,
    IRecipientsSource recipientsSource) : IMessageDispatcher
{
    public async Task SendAllAsync(NotificationDto notification)
    {
        if (notification == null) throw new ArgumentNullException(nameof(notification));

        IEnumerable<Recipient> recipients = await recipientsSource.GetAllByRecepientListAsync(notification.Recipients);
        if (recipients == null || !recipients.Any())
        {
            var errmsg = "Recipients are not defined";
            logger.LogError(errmsg);
            throw new Exception(errmsg);
        }
        foreach (IMessageSender sender in senderCollection.GetMessageSenders())
        {
            var message = new Message()
            {
                Recipients = recipients , 
                MessageText = notification.Message,
            };

            try
            {
                await sender.SendAsync(message);
            }
            catch (Exception ex)
            { 
                logger.LogError(ex.Message);
            }
        }
        
    }
}