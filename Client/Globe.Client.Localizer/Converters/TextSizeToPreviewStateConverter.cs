using Globe.Client.Localizer.Utilities;
using Globe.Client.Platform.Controls;
using Globe.Client.Platform.Services;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Globe.Client.Localizer.Converters
{
    class TextSizeToPreviewStateConverter : IMultiValueConverter
    {
        readonly TextSizeChecker _textSizeChecker = new TextSizeChecker();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var contextName = values[0] as string;
            var previewStyleService = values[1] as IPreviewStyleService;
            var text = values[2] as string;
            var textBlock = (values[3] as PreviewString).InnerTextBlock;
            var typeName = parameter as string;

            var previewStyleInfo = previewStyleService[typeName, contextName];

            text = string.IsNullOrEmpty(text) ? string.Empty : text;

            bool isValid = _textSizeChecker.Check(text, textBlock, previewStyleInfo, textBlock.Padding.Left, textBlock.Padding.Right, textBlock.Padding.Top, textBlock.Padding.Bottom, culture);

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
