using System;
using System.Windows.Media.Imaging;

namespace Globe.Client.Platform.Services.Notifications
{
    public class JobListStatusNotification : Notification
    {
        public JobListStatusNotification()
        {
            Title = "JobList Status Changed";
            Level = NotificationLevel.Info;
            Linkable = true;
            ViewToNavigate = ViewNames.CURRENT_JOB_VIEW;
            Image = new BitmapImage(new Uri("/Globe.Client.Platform.Assets;component/Icons/current_job.png", UriKind.Relative));
        }
    }
}
