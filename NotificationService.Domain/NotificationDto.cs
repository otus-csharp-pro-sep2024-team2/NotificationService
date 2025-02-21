namespace NotificationService.Domain;

public class NotificationDto 
{
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<Guid> Recipients { get; set; }
}