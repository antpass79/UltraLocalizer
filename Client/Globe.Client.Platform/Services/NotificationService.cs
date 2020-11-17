using Globe.Client.Platform.Services.Notifications;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class NotificationService : INotificationService
    {
        public NotificationService()
        {
        }

        ObservableCollection<Notification> _notifications = new ObservableCollection<Notification>();
        public IList<Notification> Notifications => _notifications;

        Notification _lastNotification;
        public Notification LastNotification => _lastNotification;

        async public Task NotifyAsync(string title, string message, NotificationLevel notificationLevel)
        {
            await NotifyAsync(new Notification
            {
                Title = title,
                Message = message,
                Level = notificationLevel
            });
        }

        async public Task NotifyAsync(Notification notification)
        {
            _notifications.Insert(0, notification);
            _lastNotification = notification;

            await Task.CompletedTask;
        }

        public void Clear()
        {
            _notifications.Clear();
            _lastNotification = null;
        }
    }
}
