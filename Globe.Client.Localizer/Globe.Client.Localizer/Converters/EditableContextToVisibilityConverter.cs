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
            var editableContext = value as EditableContext;
            if (editableContext == null)
                return Visibility.Collapsed;

            var enableLink =
                editableContext.StringId == 0 &&
                editableContext.EditableValue == null &&
                editableContext.ContextType == StringType.String ? true : false;

            return !(enableLink ^ EnableLinkMode) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
