using NotificationService.Domain.Entities;
using NotificationService.Domain.Entities.Enums;
using NotificationService.Infrastructure.Sources.Dto;

namespace NotificationService.Infrastructure.Services.Interfaces;

public interface IRecipientsSource
{
    Task<IEnumerable<Recipient>> FindAsync(Guid id);
    Task<IEnumerable<Recipient>> GetAllByRecepientListAsync(IEnumerable<Guid> notificationUserIdList);
}