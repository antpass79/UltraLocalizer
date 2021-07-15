using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MyLabLocalizer.Core.Controls
{
    public class LocNumericTextBox : LocTextBox
    {
        private static readonly Regex regex = new Regex("^[0-9]*$");

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
    }
}
