using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Entities.Enums;
using NotificationService.Infrastructure.Configuration;
using NotificationService.Infrastructure.Services.Core;
using NotificationService.Infrastructure.Services.Interfaces;

namespace NotificationService.Infrastructure.Implemetations;

public class EmailMessageSender(
    ILogger<EmailMessageSender> logger,
    IConfiguration configuration,
    ISmtpClient smtpClient,
    IEmailValidator emailValidator)
    : MessageSenderBase(logger)
{
    private readonly EmailSettings? _settings = configuration.GetSection("NotificationSettings:Email")?.Get<EmailSettings>();
    private readonly IEmailValidator _emailValidator = emailValidator;

    public override async Task SendAsync(Message message)
    {
        ArgumentNullException.ThrowIfNull(_settings);

        try
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.FromAddress),
                Subject = _settings.Subject,
                Body = message.MessageText
            };

            foreach (var email in message.Recipients.Where(p => p.ContactType == ContactType.Email))
            {
                if (_emailValidator.IsValidEmail(email.Address))
                {
                    mailMessage.To.Add(new MailAddress(email.Address));
                    await smtpClient.SendMailAsync(mailMessage);
                    logger.LogInformation($"Email sent to {email.Address}");
                }
                else
                {
                    logger.LogWarning($"Invalid email address: {email.Address}");
                }
            }

        }
        catch (Exception ex)
        {
            logger.LogError($"Error when sending an email: {ex.Message}");
            throw;
        }
    }
}