using System.Windows;
using System.Windows.Controls;

namespace MyLabLocalizer.Core.Controls
{
    public class LocCheckBox : CheckBox
    {
        public LocCheckBox()
        {
            this.Checked += LocCheckBox_Checked;
            this.Unchecked += LocCheckBox_Unchecked;
        }

        private void LocCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            IsIndeterminate = !IsChecked.HasValue;
        }

        private void LocCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            IsIndeterminate = !IsChecked.HasValue;
        }

        private static readonly DependencyPropertyKey IsIndeterminatePropertyKey
        = DependencyProperty.RegisterReadOnly(
            nameof(IsIndeterminate),
            typeof(bool), typeof(LocCheckBox),
            new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty IsIndeterminateProperty
            = IsIndeterminatePropertyKey.DependencyProperty;

        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            protected set { SetValue(IsIndeterminatePropertyKey, value); }
        }
    }
}
