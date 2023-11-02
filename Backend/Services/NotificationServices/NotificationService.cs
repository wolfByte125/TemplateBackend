
using Backend.DTOs.NotificationDTOs;
using Backend.Models.NotificationModels;
using Backend.Models.UserModels;
using Backend.Services.UserAccountService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Backend.Services.NotificationServices
{
    public class NotificationService : INotificationService
    {
        private readonly DataContext _context;
        private readonly IUserAccountService _userAccountService;

        public NotificationService(DataContext context, IUserAccountService userAccountService)
        {
            _context = context;
            _userAccountService = userAccountService;
        }

        // CLEAR NOTIFICATION
        public async Task<bool> ClearNotification(ClearNotificationDTO clearDTO)
        {
            if (clearDTO.Id == null && clearDTO.UserAccountId == null)
            {
                throw new InvalidOperationException("User Not Found.");
            }

            // CLEAR ALL NOTIFICATIONS FOR A USER
            if (clearDTO.Id == null)
            {
                List<Notification> notifications = await _context.Notifications
                    .Where(x => x.UserAccountId == clearDTO.UserAccountId)
                    .ToListAsync();

                notifications.ForEach(x => x.IsCleared = true);

                _context.Notifications.UpdateRange(notifications);
            }
            // CLEAR A SPECIFIC NOTIFICATION
            else
            {
                Notification? notification = await _context.Notifications
                    .Where(x => x.Id == clearDTO.Id)
                    .FirstOrDefaultAsync()
                    ??
                    throw new KeyNotFoundException("Notification Not Found.");

                notification.IsCleared = true;

                _context.Notifications.Update(notification);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        // READ USER'S NOTIFICATIONS
        public async Task<NotificationsAndCountReturnDTO> ReadNotifications(ReadNotificationsDTO readDTO)
        {
            UserAccount? userAccount = await _userAccountService.ReadUserAccountById(id: readDTO.UserAccountId);

            NotificationsAndCountReturnDTO returnDTO = new();

            int notificationCount = await _context.Notifications
                .Where(x =>
                    x.UserAccountId == readDTO.UserAccountId &&
                    x.IsCleared == false)
                .CountAsync();

            int limit = readDTO.Limit != null ? readDTO.Limit.Value : notificationCount;

            List<Notification> notifications = await _context.Notifications
                .Where(x => 
                    x.UserAccountId == readDTO.UserAccountId &&
                    x.IsCleared == false)
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                .ToListAsync();

            returnDTO.Count = notificationCount;
            returnDTO.Notifications = notifications;

            return returnDTO;
        }

        // GENERATE NOTIFICATION
        public async Task<Notification> GenerateNotification(GenerateNotificationDTO generateDTO)
        {
            Notification notificationContent = await MakeContent(generateDTO);

            CheckAndClearExisting(new CheckAndClearNotificationsDTO()
            {
                Type = notificationContent.Type,
                ActionId = notificationContent.ActionId
            });

            List<Notification> notifications = new();

            foreach (var userAccountId in generateDTO.UserAccountIds)
            {
                Notification notification = new();
                notification.Title = notificationContent.Title;
                notification.Message = notificationContent.Message;
                notification.Type = notificationContent.Type;
                notification.ActionId = notificationContent.ActionId;
                notification.UserAccountId = userAccountId;
                notifications.Add(notification);
            }

            _context.Notifications.AddRange(notifications);
            await _context.SaveChangesAsync();

            return notificationContent;
        }

        // CHECK AND CLEAR ALREADY EXISTING NOTIFICATION THAT IS THE SAME
        private async void CheckAndClearExisting(CheckAndClearNotificationsDTO checkAndClearDTO)
        {
            // FETCH EXISTING NOTIFICATIONS
            List<Notification> existingNotifications = await _context.Notifications
                .Where(x =>
                    x.Type == checkAndClearDTO.Type &&
                    x.ActionId == checkAndClearDTO.ActionId)
                .ToListAsync();

            // CLEAR OUT NOITIFICATIONS
            if (existingNotifications.Count > 0)
                existingNotifications.AsParallel().ForAll(n => n.IsCleared = true);
        }

        private async Task<Notification> MakeContent(GenerateNotificationDTO generateDTO)
        {
            Notification notification = new();

            switch (generateDTO.NotificationDetail)
            {
                // DEMO NOTIFICATION
                case NOTIFICATION_DETAIL.DEMO_NOTIFICATION:
                    notification.Type = NOTIFICATION_TYPE.DEMO;
                    notification.Title = NOTIFICATION_DETAIL.DEMO_NOTIFICATION;
                    notification.Message = $"This is a demo notification.";
                    notification.ActionId = generateDTO.ActionId;
                    break;

                default:
                    notification.Type = "";
                    notification.Title = "";
                    notification.Message = "";
                    break;
            }

            return notification;
        }

    }
}
