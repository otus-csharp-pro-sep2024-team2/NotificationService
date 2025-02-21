using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationService.Infrastructure.Implemetations;

public class MessageConsumer : BackgroundService, IMessageConsumer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MessageConsumer> _logger;
    private readonly string QueueName;
    

    public MessageConsumer(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<MessageConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        var brokerConnection = configuration.GetSection("BrokerConnection").Get<BrokerConnection>();
        if (brokerConnection == null)
            throw new ArgumentException("Broker connection string is missing");
        var factory = new ConnectionFactory
        {
            HostName = brokerConnection.HostName,
            Port = brokerConnection.Port,
            UserName = brokerConnection.UserName,
            Password = brokerConnection.Password, 
            DispatchConsumersAsync = true 
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        QueueName = brokerConnection.Queue;
        _channel.QueueDeclare(queue: QueueName,
                              durable: false,
                              exclusive: false,
                              autoDelete: false);
    }
    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
    public void StartConsuming()
    {
        _logger.LogInformation("Waiting for messages.");

        //var consumer = new EventingBasicConsumer(_channel);
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += OnMessageReceivedAsync;
        _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
        _logger.LogInformation($"Notification service started: {DateTime.Now} / Queue: {QueueName}");
    }

    private async Task OnMessageReceivedAsync(object? sender, BasicDeliverEventArgs ea)
    {
        try
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Message received: {message}");

            // Create a scope to get dependencies (e.g., INotificationService)
            using (var scope = _serviceProvider.CreateScope())
            {
                var messageDispatcher = scope.ServiceProvider.GetRequiredService<IMessageDispatcher>();
                var notificationConverter = scope.ServiceProvider.GetRequiredService<INotificationConverter>();
                var notification = notificationConverter.Convert(message);
                await messageDispatcher.SendAllAsync(notification);
            }

            _channel.BasicAck(ea.DeliveryTag, multiple: false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
        }
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        StartConsuming();
        return Task.CompletedTask;
    }

}