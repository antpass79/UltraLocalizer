using Globe.Client.Localizer.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Globe.Client.Localizer.Converters
{
    public class LinkableItemVisibilityConverter : IValueConverter
    {
        public bool EnableLinkMode { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var linkableItem = value as LinkableItem;
            if (linkableItem == null || linkableItem.EditableStringItem == null || linkableItem.ConceptViewItem == null)
                return Visibility.Collapsed;

            var enableLink =
                linkableItem.EditableStringItem.StringId == 0 &&
                linkableItem.EditableStringItem.ContextValue == null &&
                linkableItem.EditableStringItem.ContextType == StringType.String ? true : false;

            return !(enableLink ^ EnableLinkMode) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
