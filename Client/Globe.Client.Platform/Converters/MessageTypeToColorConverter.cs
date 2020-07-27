using Globe.Client.Platofrm.Events;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Globe.Client.Platform.Converters
{
    public sealed class MessageTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MessageType messageType = (MessageType)value;

            switch (messageType)
            {
                case MessageType.None:
                    return new SolidColorBrush(Colors.Transparent);
                case MessageType.Error:
                    return new SolidColorBrush(Colors.Red);
                case MessageType.Warning:
                    return new SolidColorBrush(Colors.Orange);
                case MessageType.Information:
                default:
                    return new SolidColorBrush(Colors.Green);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
