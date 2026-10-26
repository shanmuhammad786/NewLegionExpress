using legionexpress.Models;
using System.Threading.Tasks;

namespace legionexpress.Services
{
    internal class NotificationApiService : MobileServiceBase
    {
        public async Task<UserNotificationsResponse> GetUserNotifications()
        {
            string url = "api/v1/notification/get-user-notifications";
            return await GetAuthorized<UserNotificationsResponse>(url);
        }

        public async Task<AcknowledgeNotificationResponse> SetNotificationAcknowledged(int notificationLogId)
        {
            string url = "api/v1/notification/set-notification-acknowledged";
            var request = new AcknowledgeNotificationRequest { Id = notificationLogId };
            return await PostAuthorizedRequest<AcknowledgeNotificationResponse, AcknowledgeNotificationRequest>(request, url);
        }
    }
}
