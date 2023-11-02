using Backend.DTOs.NotificationDTOs;
using Backend.Models.NotificationModels;

namespace Backend.Services.NotificationServices
{
    public interface INotificationService
    {
        Task<bool> ClearNotification(ClearNotificationDTO clearDTO);
        Task<Notification> GenerateNotification(GenerateNotificationDTO generateDTO);
        Task<NotificationsAndCountReturnDTO> ReadNotifications(ReadNotificationsDTO readDTO);
    }
}
