using NotificationService.Domain.Entities.Enums;

namespace NotificationService.Domain.Entities;

public sealed class Recipient() : Entity<Guid>(Guid.NewGuid())
{
    public Guid RecipientId { get; set; }
    public ContactType ContactType { get; set; }
    public string Address { get; set; }
    public string Lastname { get; set; }
    public string Firstname { get; set; }
    
    public IEnumerable<NotificationRecepientLink> Links { get; set; } = new List<NotificationRecepientLink>();
}
