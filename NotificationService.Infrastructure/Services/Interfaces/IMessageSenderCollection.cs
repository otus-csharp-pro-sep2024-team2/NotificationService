namespace NotificationService.Infrastructure.Services.Interfaces;

public interface IMessageSenderCollection
{
    public IEnumerable<IMessageSender> GetMessageSenders();
}
