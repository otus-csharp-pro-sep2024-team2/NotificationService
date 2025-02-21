using System.Net.Mail;
using NotificationService.Infrastructure.Services.Interfaces;

namespace NotificationService.Infrastructure.Services.Implementations;

public class EmailValidator : IEmailValidator
{
    public bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}