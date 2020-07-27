using Globe.Client.Platform.Controls;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Globe.Client.Localizer.Converters
{
    class ContextNameToPreviewStyleInfoConverter : IValueConverter
    {
        PreviewStyleService _previewStyleService = new PreviewStyleService();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var contextName = value as string;
            return _previewStyleService[contextName];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
