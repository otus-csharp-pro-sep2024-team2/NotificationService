namespace NotificationService.Infrastructure.Core;

public class NotificationResponse
{
    public string Id { get; set; }
    public string Status { get; set; }
    public DateTime SentAt { get; set; }
    public override string ToString()
    {
        return $"{Id}-{Status}-{SentAt}";
    }
}