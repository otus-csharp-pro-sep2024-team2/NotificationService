namespace NotificationService.Domain.ValueObjects;

public class PersonName
{
    public string Name { get; }
    public PersonName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));
        Name = name;
    }
    public override string ToString()
    {
        return $"{Name}";
    }    
}