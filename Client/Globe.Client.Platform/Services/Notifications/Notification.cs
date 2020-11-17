using Globe.Client.Platform.ViewModels;
using System;
using System.Windows.Media;

namespace Globe.Client.Platform.Services.Notifications
{
    public enum NotificationLevel
    {
        Ok,
        Info,
        Warning,
        Error
    }

    public class Notification : PropertyChangedNotifier
    {
        #region Properties

        string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        string _message = string.Empty;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        NotificationLevel _level = NotificationLevel.Ok;
        public NotificationLevel Level
        {
            get { return _level; }
            set
            {
                _level = value;
                OnPropertyChanged("Level");
            }
        }
        
        DateTime _notificationTime = DateTime.UtcNow;
        public DateTime NotificationTime
        {
            get { return _notificationTime; }
            set
            {
                _notificationTime = value;
                OnPropertyChanged("NotificationTime");
            }
        }

        bool _linkable = false;
        public bool Linkable
        {
            get { return _linkable; }
            set
            {
                _linkable = value;
                OnPropertyChanged("Linkable");
            }
        }

        bool _isManualRemoveable = false;
        public bool IsManualRemoveable
        {
            get { return _isManualRemoveable; }
            set
            {
                _isManualRemoveable = value;
                OnPropertyChanged("IsManualRemoveable");
            }
        }

        string _viewToNavigate = string.Empty;
        public string ViewToNavigate
        {
            get { return _viewToNavigate; }
            set
            {
                _viewToNavigate = value;
                OnPropertyChanged("ViewToNavigate");
            }
        }

        ImageSource _image = null;

        public ImageSource Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        #endregion
    }
}
