using Microsoft.AspNetCore.Mvc;
using NotificationService.Domain;
using NotificationService.Infrastructure.Services.Interfaces;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController(IMessageDispatcher messsageDispatcher) : ControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] NotificationDto dto)
    {
        // The simplest validation and creation of a domain model
        await messsageDispatcher.SendAllAsync(dto);
        return Ok("The notification has been sent");
    }
}


