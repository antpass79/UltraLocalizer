using MyLabLocalizer.Core.Services.Notifications;
using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MyLabLocalizer.Core.Converters
{
    public class NotificationLevelToImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            NotificationLevel level = (NotificationLevel)value;
            switch (level)
            {
                case NotificationLevel.Error:
                    return new BitmapImage(new Uri("/MyLabLocalizer.Core;component/Icons/error.png", UriKind.Relative));
                case NotificationLevel.Warning:
                    return new BitmapImage(new Uri("/MyLabLocalizer.Core;component/Icons/warning.png", UriKind.Relative));
                case NotificationLevel.Info:
                    return new BitmapImage(new Uri("/MyLabLocalizer.Core;component/Icons/information.png", UriKind.Relative));
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
