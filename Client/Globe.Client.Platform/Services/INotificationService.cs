using Globe.Client.Platform.Services.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public interface INotificationService
    {
        IList<Notification> Notifications
        {
            get;
        }

        Notification LastNotification
        {
            get;
        }

        Task NotifyAsync(string title, string message, NotificationLevel notificationLevel);
        Task NotifyAsync(Notification notification);

        void Clear();
    }
}
