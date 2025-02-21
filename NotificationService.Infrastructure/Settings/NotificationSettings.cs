namespace NotificationService.Infrastructure.Configuration;

public class NotificationSettings
{
    public required EmailSettings Email { get; set; }
    public required TelegramSettings Telegram { get; set; }
}