namespace NotificationService.Infrastructure.Configuration;

public class EmailSettings
{
    public required string SmtpServer { get; set; }
    public required int Port { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public required string FromAddress { get; set; }
    public string? Subject {get; set; }
}