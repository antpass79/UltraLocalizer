using MyLabLocalizer.Core.Services.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Core.Services
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
