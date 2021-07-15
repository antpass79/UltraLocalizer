using System;
using System.Globalization;
using System.Windows.Data;

namespace MyLabLocalizer.Core.Converters
{
    public class ImageNameToSourceImageConverter : IValueConverter
    {
        public string AssemblyName { get; set; } = "MyLabLocalizer.Core";
        public string ImageFolderPath { get; set; } = "Icons";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = $"/{AssemblyName};component/{ImageFolderPath}/{value}.png";
            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
