namespace NotificationService.Infrastructure.Configuration;

public record TelegramSettings(string token)
{
    public string Token => token;
}
