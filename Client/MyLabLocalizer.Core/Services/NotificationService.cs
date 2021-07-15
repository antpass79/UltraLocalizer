using MyLabLocalizer.Core.Services.Notifications;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MyLabLocalizer.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IDialogService _dialogService;

        public NotificationService(IDialogService dialogService)
        {
            _dialogService = dialogService;
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

            if (_lastNotification.Level == NotificationLevel.Error)
            {
                var @params = new DialogParameters();

                @params.Add("notification", notification);

                _dialogService.ShowDialog("MessageDialog", @params, dialogResult =>
                {
                });
            }

            await Task.CompletedTask;
        }

        public void Clear()
        {
            _notifications.Clear();
            _lastNotification = null;
        }
    }
}
