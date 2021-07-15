using MyLabLocalizer.Utilities;
using MyLabLocalizer.Core.Controls;
using MyLabLocalizer.Core.Services;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MyLabLocalizer.Converters
{
    class TextSizeToPreviewStateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var contextName = values[0] as string;
            var previewStyleService = values[1] as IPreviewStyleService;
            var text = values[2] as string;
            var textBlock = (values[3] as PreviewString).InnerTextBlock;
            var typeName = parameter as string;

            var previewStyleInfo = previewStyleService[typeName, contextName];

            text = string.IsNullOrEmpty(text) ? string.Empty : text;

            bool isValid = TextSizeChecker.Check(text, textBlock, previewStyleInfo, textBlock.Padding.Left, textBlock.Padding.Right, textBlock.Padding.Top, textBlock.Padding.Bottom, culture);

            if (isValid)
                return PreviewState.Valid;
            else
                return PreviewState.Invalid;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
