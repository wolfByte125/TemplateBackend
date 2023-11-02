using Microsoft.Identity.Client;

namespace Backend.DTOs.NotificationDTOs
{
    public class ClearNotificationDTO
    {
        public int? Id { get; set; }

        public string? UserAccountId { get; set; }
    }
}
