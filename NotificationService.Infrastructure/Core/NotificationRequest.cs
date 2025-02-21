namespace NotificationService.Infrastructure.Core;

public abstract record NotificationRequest(string recipient, string mesage, string name)
{
    
    public string Message => mesage;
    public string Recipient => recipient;
    public string Name => name;
}