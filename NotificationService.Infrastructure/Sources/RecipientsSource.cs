using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Entities.Enums;
using NotificationService.Infrastructure.Services.Interfaces;
using NotificationService.Infrastructure.Sources.Dto;
using NotificationService.Infrastructure.Sources.Exceptions;

namespace NotificationService.Infrastructure.Sources;

public class RecipientsSource : IRecipientsSource
{
    private string _userByIdEndpoint;
    private string _baseAddress;
    private readonly string _serviceName;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RecipientsSource> _logger;
    private readonly IConfiguration _configuration;
    private static JsonSerializerOptions _serializerOptions;

    public RecipientsSource(IHttpClientFactory httpClientFactory,
        string serviceName,
        ILogger<RecipientsSource> logger,
        IConfiguration configuration)
    {
        _configuration = configuration;
        SetProfileServiceConnection();
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _serviceName = string.IsNullOrWhiteSpace(serviceName) ? throw new ArgumentException(nameof(serviceName)) : serviceName;
    }

    private void SetProfileServiceConnection()
    {
        var profileService = _configuration.GetSection("ProfileService").Get<ProfileServiceConnection>();
        if (profileService == null)
            throw new ArgumentException("ProfileService connection string is missing");

        _baseAddress = profileService.BaseAddress;
        if (string.IsNullOrEmpty(_baseAddress))
            throw new ArgumentException("ProfileService.BaseAddress is missing");

        _userByIdEndpoint = profileService.UserByIdEndpoint; 
        if (string.IsNullOrEmpty(_userByIdEndpoint))
            throw new ArgumentException("ProfileService.UserByIdEndpoint is missing");
            
    }

    public async Task<IEnumerable<Recipient>> FindAsync(Guid userId)
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient(_serviceName);
        httpClient.BaseAddress = new Uri(_baseAddress);
        var requestUri = new UriBuilder(httpClient.BaseAddress)
        {
            Path = string.Format(_userByIdEndpoint, userId)
        }.Uri;
        _logger.LogInformation("Attempt to request for URL: {Url}", requestUri);
        HttpResponseMessage response;
        try
        {
            response = await httpClient.GetAsync(requestUri);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UserService is unavailable");
            throw;
        }

        // Проверяем успешность запроса
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for URL: {Url}", requestUri);
            throw new HttpRequestException(ex.Message, ex);
        }

        // Десериализуем ответ
        RecipientDto recipientDto;
        try
        {
            string content = await response.Content.ReadAsStringAsync();
            if (content is null)
            {
                _logger.LogError("Invalid service response for URL: {Url}", requestUri);
                throw new InvalidServiceResponseException();
            }
            
            recipientDto = JsonSerializer.Deserialize<RecipientDto>(content, GetJsonSerializerOptions());  //await response.Content.ReadFromJsonAsync<RecipientDto>(GetJsonSerializerOptions());
            if (recipientDto is null)
            {
                _logger.LogError("Invalid service response for URL: {Url}", requestUri);
                throw new InvalidServiceResponseException();
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize response for URL: {Url}", requestUri);
            throw new InvalidServiceResponseException("Invalid JSON response", ex);
        }

        _logger.LogInformation("Successfully processed request for URL: {Url}", requestUri);

        // Создаем список получателей
        var recipients = new List<Recipient>();

        // Добавляем email, если он есть
        if (!string.IsNullOrEmpty(recipientDto.Email))
        {
            var recipient = CreateRecipient(recipientDto.Id, recipientDto.Email, ContactType.Email, recipientDto.FirstName,
                recipientDto.LastName);
            recipients.Add(recipient);
        }

        // Добавляем Telegram ID, если он есть
        if (!string.IsNullOrEmpty(recipientDto.TelegramId))
        {
            var recipient = CreateRecipient(recipientDto.Id, recipientDto.TelegramId, ContactType.Telegram, recipientDto.FirstName,
                recipientDto.LastName);
            recipients.Add(recipient);
        }

        return recipients;
    }
    private Recipient CreateRecipient(Guid recipientId, string address, ContactType contactType, string firstName, string lastName)
    {
        return new Recipient
        {
            RecipientId = recipientId,
            Address = address,
            ContactType = contactType,
            Lastname = lastName,
            Firstname = firstName,
        };
    }    
    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        if (_serializerOptions != null)
        {
            return _serializerOptions;
        }

        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        _serializerOptions.Converters.Add(new JsonStringEnumConverter());
        
        return _serializerOptions;
    }

    public async Task<IEnumerable<Recipient>> GetAllByRecepientListAsync(IEnumerable<Guid> userIdList)
    {
        var result = new List<Recipient>();
        foreach (var userId in userIdList)
        {
            var recipient = await FindAsync(userId);
            result.AddRange(recipient);
        }
        return result;
    }
}