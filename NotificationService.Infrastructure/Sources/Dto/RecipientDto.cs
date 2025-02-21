namespace NotificationService.Infrastructure.Sources.Dto;

public class RecipientDto
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string TelegramId { get; set; }
    public string Email { get; set; }
    public Guid Id { get; set; }
}