using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Hosting.Server;

namespace Backend.DTOs.NotificationDTOs
{
    public class CheckAndClearNotificationsDTO
    {
        public string Type { get; set; } = null!;

        public int ActionId { get; set; }
    }
}
