using NotificationService.Domain.Entities.Enums;

namespace NotificationService.Domain.Entities;

public class Notification : Entity<Guid>
{
    public NotificationType Type { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }

    public Notification(Guid id, NotificationType type, string message, DateTime createdAt)
        : base(id)
    {
        Type = type;
        Message = message;
        CreatedAt = createdAt;
    }

    public Notification()
        : this(Guid.Empty, NotificationType.Default, "Empty", DateTime.Now)
    {
    }
    public IEnumerable<NotificationRecepientLink> Links { get; set; } = new List<NotificationRecepientLink>();
}
