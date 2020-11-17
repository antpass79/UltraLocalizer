using System;
using System.Windows.Media.Imaging;

namespace Globe.Client.Platform.Services.Notifications
{
    public class JobListStatusNotification : Notification
    {
        public JobListStatusNotification(string message)
        {
            Title = "JobList Status Changed";
            Message = message;
            Level = NotificationLevel.Info;
            Linkable = true;
            ViewToNavigate = ViewNames.CURRENT_JOB_VIEW;
            Image = new BitmapImage(new Uri("/Globe.Client.Platform.Assets;component/Icons/current_job.png", UriKind.Relative));
        }
    }
}
