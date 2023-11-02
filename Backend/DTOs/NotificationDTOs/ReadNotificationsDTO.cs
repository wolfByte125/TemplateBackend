namespace Backend.DTOs.NotificationDTOs
{
    public class ReadNotificationsDTO
    {
        public string UserAccountId { get; set; } = null!;

        public int? Limit { get; set; }
    }
}
