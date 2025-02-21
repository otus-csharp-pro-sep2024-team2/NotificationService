using System.Net.Mail;

namespace NotificationService.Infrastructure.Services.Interfaces;

public interface ISmtpClient
{
    Task SendMailAsync(MailMessage message);    
}