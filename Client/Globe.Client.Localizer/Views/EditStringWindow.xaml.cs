using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Globe.Client.Localizer.Views
{
    /// <summary>
    /// Interaction logic for EditStringWindow.xaml
    /// </summary>
    public partial class EditStringWindow : UserControl
    {
        public EditStringWindow()
        {
            InitializeComponent();
        }

        private void LocTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            var _regex = new Regex("[^0-9.-]+");
            return !_regex.IsMatch(text);
        }
    }
}
