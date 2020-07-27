using System;
using System.Globalization;
using System.Windows.Data;

namespace Globe.Client.Platform.Converters
{
    public class NullValueToBooleanConverter : IValueConverter
    {
        public bool Inverted { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && !Inverted ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
