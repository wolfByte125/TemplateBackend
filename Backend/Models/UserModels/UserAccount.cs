using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Models.UserModels
{
    public class UserAccount : IAuditableEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Username { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [JsonIgnore]
        public byte[] PasswordHash { get; set; }

        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }

        public string Status { get; set; } = USER_STATUS.NEW;

        public int CountBans { get; set; } = 0;

        public UserRole UserRole { get; set; }
        
        [ForeignKey("UserRole")]
        public int UserRoleId { get; set; }

        // BASIC INFO

        public string FirstName { get; set; } = null!;

        public string MiddleName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string FullName => FirstName + " " + MiddleName + " " + LastName;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
