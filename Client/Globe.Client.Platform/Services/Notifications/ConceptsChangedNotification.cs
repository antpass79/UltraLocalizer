using System;
using System.Windows.Media.Imaging;

namespace Globe.Client.Platform.Services.Notifications
{
    public class ConceptsChangedNotification : Notification
    {
        public ConceptsChangedNotification()
        {
            Title = "New Concepts";
            Level = NotificationLevel.Info;
            Linkable = true;
            ViewToNavigate = ViewNames.JOBLIST_MANAGEMENT_VIEW;
            Image = new BitmapImage(new Uri("/Globe.Client.Platform.Assets;component/Icons/management.png", UriKind.Relative));
        }
    }
}
