using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyLabLocalizer.Core.Behaviours
{
    public class TextChangedBehaviour
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand),
        typeof(TextChangedBehaviour), new PropertyMetadata(PropertyChangedCallback));

        public static void PropertyChangedCallback(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            TextBox textBox = (TextBox)depObj;
            if (textBox != null)
            {
                textBox.TextChanged += new TextChangedEventHandler(TextChanged);
            }
        }

        public static ICommand GetCommand(UIElement element)
        {
            return (ICommand)element.GetValue(CommandProperty);
        }

        public static void SetCommand(UIElement element, ICommand command)
        {
            element.SetValue(CommandProperty, command);
        }

        private static void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox != null)
            {
                ICommand command = textBox.GetValue(CommandProperty) as ICommand;
                if (command != null)
                {
                    command.Execute(textBox.Text);
                }
            }
        }
    }
}