using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Services.Interfaces;

namespace NotificationService.Infrastructure.Implemetations;

public class MessageSenderCollection(IServiceProvider serviceProvider) : IMessageSenderCollection
{
    public IEnumerable<IMessageSender> GetMessageSenders()
    {
        return serviceProvider.GetServices<IMessageSender>();
    }
}
