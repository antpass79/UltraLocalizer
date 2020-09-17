using Globe.Client.Localizer.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Globe.Client.Localizer.Converters
{
    public class EditableContextToVisibilityConverter : IValueConverter
    {
        public bool EnableLinkMode { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringId = (int)value;

            var enableLink =
                stringId == 0 ? true : false;

            return !(enableLink ^ EnableLinkMode) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
