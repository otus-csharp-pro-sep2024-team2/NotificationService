namespace NotificationService.Domain.Entities;

public class NotificationRecepientLink
{
    public Guid NotificationId { get; set; }
    public Guid RecepientId { get; set; }
}