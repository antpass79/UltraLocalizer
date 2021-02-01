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
            return level switch
            {
                NotificationLevel.Error => new SolidColorBrush(Colors.Red),
                NotificationLevel.Warning => new SolidColorBrush(Colors.Orange),
                _ => new SolidColorBrush(Colors.Green),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
