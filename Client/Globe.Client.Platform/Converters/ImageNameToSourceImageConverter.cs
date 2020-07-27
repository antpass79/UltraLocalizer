using System;
using System.Globalization;
using System.Windows.Data;

namespace Globe.Client.Platform.Converters
{
    public class ImageNameToSourceImageConverter : IValueConverter
    {
        public string AssemblyName { get; set; } = "Globe.Client.Platform.Assets";
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
