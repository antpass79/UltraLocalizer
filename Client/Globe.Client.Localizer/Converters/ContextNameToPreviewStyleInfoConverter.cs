using Globe.Client.Platform.Services;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Globe.Client.Localizer.Converters
{
    class ContextNameToPreviewStyleInfoConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var contextName = values[0] as string;
            var previewStyleService = values[1] as IPreviewStyleService;
            var typeName = parameter as string;
            
            return previewStyleService[typeName, contextName];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
