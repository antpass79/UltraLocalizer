using System.Windows.Media;

namespace Globe.Client.Platform.Services.Notifications
{
    public class Notification
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationLevel Level { get; set; } = NotificationLevel.Info;        
        public bool Linkable { get; protected set; }
        public string ViewToNavigate { get; protected set; }
        public ImageSource Image { get; protected set; }
    }
}
