namespace NotificationService.Domain.Entities;

public sealed class Message
{
    public IEnumerable<Recipient> Recipients { get; set; }
    public string MessageText { get; set; }
}
