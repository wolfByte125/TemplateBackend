using System.ComponentModel.DataAnnotations;

namespace Backend.Models.NotificationModels
{
    public class Notification : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        
        public string Message { get; set; } = string.Empty;

        public string Type { get; set; } = null!;

        public int ActionId { get; set; }

        public string UserAccountId { get; set; } = null!;

        public bool IsCleared { get; set; } = false;

        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

    }
}
