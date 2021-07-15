using System;
using System.Windows.Media.Imaging;

namespace MyLabLocalizer.Core.Services.Notifications
{
    public class JobListStatusNotification : Notification
    {
        public JobListStatusNotification()
        {
            Title = "JobList Status Changed";
            Level = NotificationLevel.Info;
            Linkable = true;
            ViewToNavigate = ViewNames.CURRENT_JOB_VIEW;
            Image = new BitmapImage(new Uri("/MyLabLocalizer.Core;component/Icons/current_job.png", UriKind.Relative));
        }
    }
}
