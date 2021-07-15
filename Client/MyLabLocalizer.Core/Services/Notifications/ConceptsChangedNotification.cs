using System;
using System.Windows.Media.Imaging;

namespace MyLabLocalizer.Core.Services.Notifications
{
    public class ConceptsChangedNotification : Notification
    {
        public ConceptsChangedNotification()
        {
            Title = "New Concepts";
            Level = NotificationLevel.Info;
            Linkable = true;
            ViewToNavigate = ViewNames.JOBLIST_MANAGEMENT_VIEW;
            Image = new BitmapImage(new Uri("/MyLabLocalizer.Core;component/Icons/management.png", UriKind.Relative));
        }
    }
}
