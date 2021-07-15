using System;
using System.Globalization;
using System.Windows.Data;

namespace MyLabLocalizer.Converters
{
    class IsEnglishLinkEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isLinked = (bool)values[0];
            var isEnglish = (bool)values[1];

            return isEnglish && !isLinked;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
