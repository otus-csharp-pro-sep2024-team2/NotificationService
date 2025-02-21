using System.Net;
using System.Net.Mail;
using NotificationService.Infrastructure.Configuration;
using NotificationService.Infrastructure.Implemetations;
using NotificationService.Infrastructure.Services.Implementations;
using NotificationService.Infrastructure.Services.Interfaces;
using NotificationService.Infrastructure.Sources;

namespace NotificationService.API.Extension;

public static class ServiceCollectionExtensions
{
    private static IServiceCollection AddRecepientsSource(this IServiceCollection serviceDescriptors,
        string serviceName,
        IConfiguration configuration)
    {
        return serviceDescriptors.AddTransient<IRecipientsSource>(provider =>
        {
            IHttpClientFactory httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            ILogger<RecipientsSource> logger = provider.GetRequiredService<ILogger<RecipientsSource>>();
            
            return new RecipientsSource(httpClientFactory, serviceName, logger, configuration);
        });
    }    
    public static IServiceCollection AddNotificationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<NotificationSettings>(
            configuration.GetSection("NotificationSettings"));

        services.AddSingleton<SmtpClient>(provider =>
        {
            var emailSettings = configuration.GetSection("NotificationSettings:Email").Get<EmailSettings>() ?? throw new ArgumentNullException("configuration.GetSection(\"NotificationSettings:Email\").Get<EmailSettings>()");
            var client = new SmtpClient(emailSettings.SmtpServer, emailSettings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password)
            };

            return client;
        });
        services.AddRecepientsSource("ProfileService", configuration);
        services.AddTransient<INotificationConverter, JsonNotificationConverter>();
        services.AddScoped<IMessageSenderCollection, MessageSenderCollection>();
        services.AddTransient<IMessageConsumer, MessageConsumer>(); 
        services.AddTransient<IMessageDispatcher, MessageDispatcher>(); 
        services.AddTransient<IMessageSender,EmailMessageSender>();
        services.AddTransient<IMessageSender,TelegramMessageSender>();
        services.AddSingleton<ISmtpClient, SmtpClientWrapper>();
        services.AddSingleton<IEmailValidator, EmailValidator>();
        return services;
    }
}
