namespace NotificationService.Infrastructure.Services.Interfaces;

public interface IEmailValidator
{
    bool IsValidEmail(string email);
}