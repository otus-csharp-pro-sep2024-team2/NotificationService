using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Entities.Enums;
using NotificationService.Infrastructure.Configuration;
using NotificationService.Infrastructure.Services.Core;

namespace NotificationService.Infrastructure.Implemetations;

public class TelegramMessageSender(
    ILogger<TelegramMessageSender> logger,
    IConfiguration configuration)
    : MessageSenderBase(logger)
{
    private readonly TelegramSettings? _settings = configuration.GetSection("NotificationSettings:Telegram").Get<TelegramSettings>();
    private static readonly HttpClient client = new();

    public override async Task SendAsync(Message message)
    {
        ArgumentNullException.ThrowIfNull(_settings);
            foreach (var recipient in message.Recipients.Where(p=>p.ContactType == ContactType.Telegram))
            {
                try
                {
                    string escapedMessage = Uri.EscapeDataString(message.MessageText);
                    string url = $"https://api.telegram.org/bot{_settings.Token}/sendMessage?chat_id={recipient.Address}&text={escapedMessage}";
                    HttpResponseMessage response = await client.GetAsync(url);
                    try
                    {
                        response.EnsureSuccessStatusCode();
                    }
                    catch (HttpRequestException ex)
                    {
                        logger.LogError(ex, "HTTP request failed for URL: {Url}", url);
                        throw new HttpRequestException(ex.Message, ex);
                    }
                    string responseString = await response.Content.ReadAsStringAsync();
                    logger.LogInformation($"Message sent to {recipient.RecipientId}: {responseString}");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error when sending telegram message: " + ex.Message);
                    throw;
                }        
            }
    }
}