namespace NotificationService.Domain.Entities;
public abstract class Entity<TId>
{
    public TId Id { get; set; }

    protected Entity(TId id)
    {
        Id = id;
    }
}

