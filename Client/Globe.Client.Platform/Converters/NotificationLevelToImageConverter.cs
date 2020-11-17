using Globe.Client.Platform.Services.Notifications;
using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Globe.Client.Platform.Converters
{
    public class NotificationLevelToImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            NotificationLevel level = (NotificationLevel)value;
            switch (level)
            {
                case NotificationLevel.Ok:
                    return new BitmapImage(new Uri("/Globe.Client.Platform.Assets;component/Icons/check.png", UriKind.Relative));
                case NotificationLevel.Error:
                    return new BitmapImage(new Uri("/Globe.Client.Platform.Assets;component/Icons/error.png", UriKind.Relative));
                case NotificationLevel.Warning:
                    return new BitmapImage(new Uri("/Globe.Client.Platform.Assets;component/Icons/warning.png", UriKind.Relative));
                case NotificationLevel.Info:
                    return new BitmapImage(new Uri("/Globe.Client.Platform.Assets;component/Icons/information.png", UriKind.Relative));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
