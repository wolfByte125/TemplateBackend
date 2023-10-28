using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models.UserModels
{
    public class UserRole : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public string RoleName { get; set; } = null!;

        public bool IsSuperAdmin { get; set; } = false;

        public Permissions Permissions { get; set; } = new();

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }
    }
}
