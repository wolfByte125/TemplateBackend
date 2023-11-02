using Backend.DTOs.NotificationDTOs;
using Backend.Services.NotificationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AUTHORIZATION.EXCLUDE_INACTIVE)]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<ActionResult> ClearNotification(ClearNotificationDTO clearDTO)
        {
            try
            {
                return Ok(await _notificationService.ClearNotification(clearDTO: clearDTO));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ReadNotifications([FromQuery] ReadNotificationsDTO readDTO)
        {
            try
            {
                return Ok(await _notificationService.ReadNotifications(readDTO: readDTO));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }
    }
}
