using System;
using System.Windows.Media.Imaging;

namespace MyLabLocalizer.Core.Services.Notifications
{
    public class DownloadCompletedNotification : Notification
    {
        public DownloadCompletedNotification()
        {
            Title = "Information";
            Message = "Download Completed";
            Level = NotificationLevel.Error;
            Linkable = true;
            ViewToNavigate = ViewNames.JOBLIST_MANAGEMENT_VIEW;
            Image = new BitmapImage(new Uri("/MyLabLocalizer.Core;component/Icons/management.png", UriKind.Relative));
        }
    }
}
