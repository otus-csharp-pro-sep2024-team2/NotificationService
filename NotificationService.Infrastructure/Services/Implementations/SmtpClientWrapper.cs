using System.Net.Mail;
using NotificationService.Infrastructure.Services.Interfaces;

namespace NotificationService.Infrastructure.Services.Implementations;

public class SmtpClientWrapper(SmtpClient client): ISmtpClient
{
    public Task SendMailAsync(MailMessage message)
    {
        return client.SendMailAsync(message);
    }   
}