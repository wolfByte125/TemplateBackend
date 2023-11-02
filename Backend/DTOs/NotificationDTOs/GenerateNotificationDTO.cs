namespace Backend.DTOs.NotificationDTOs
{
    public class GenerateNotificationDTO
    {
        public int ActionId { get; set; }

        public List<string>? UserAccountIds { get; set; }

        public string NotificationDetail { get; set; } = null!;
    }
}
