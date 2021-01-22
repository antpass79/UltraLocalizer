using Globe.Client.Platform.Controls;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Globe.Client.Platform.Converters
{
    public class PreviewStateToBorderBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var previewState = (PreviewState)value;
            return previewState == PreviewState.Valid ? Brushes.Azure : Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
