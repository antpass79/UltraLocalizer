using Globe.Client.Platform.Services.Notifications;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Globe.Client.Platform.Converters
{
    public class NotificationToColorConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            NotificationLevel level = (NotificationLevel)value;
            switch (level)
            {
                case NotificationLevel.Error:
                    return new SolidColorBrush(Colors.Red);
                case NotificationLevel.Warning:
                    return new SolidColorBrush(Colors.Orange);
                case NotificationLevel.Ok:
                case NotificationLevel.Info:
                default:
                    return new SolidColorBrush(Colors.Green);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
