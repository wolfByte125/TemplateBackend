using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models.UserModels
{
    public class Permissions
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
    }
}
