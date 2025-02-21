using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using NotificationService.Domain;
using NotificationService.Infrastructure.Services.Interfaces;

namespace NotificationService.Infrastructure.Services.Implementations;

public class JsonNotificationConverter : INotificationConverter
{
    public NotificationDto Convert(string message)
    {
        try
        { 
            var jsonOptions = new System.Text.Json.JsonSerializerOptions();
            jsonOptions.Converters.Add(new JsonStringEnumConverter());
            var messageDto = JsonSerializer.Deserialize<NotificationDto>(message, jsonOptions);
            if (messageDto == null)
                throw new Exception($"Invalid format of notification message: {message}");
            return messageDto;
        } 
        catch (JsonException ex)
        {
            throw new SerializationException(ex.Message, ex);
        }
    }
}
