using Globe.Client.Platform.Extensions;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace Globe.Client.Platform.Controls
{
    /// <summary>
    /// Interaction logic for PasswordInput.xaml
    /// </summary>
    public partial class PasswordInput : UserControl
    {
        public PasswordInput()
        {
            InitializeComponent();

            PasswordBox.PasswordChanged += (sender, args) =>
            {
                Password = ((PasswordBox)sender).SecurePassword;
            };
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(SecureString), typeof(PasswordInput), new PropertyMetadata(default(SecureString), OnPasswordPropertyChanged));
        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordInput = d as PasswordInput;

            var secureString = e.NewValue as SecureString;
            var newPassword = secureString.ToPlainString();
            if (passwordInput.PasswordBox.Password != newPassword)
                passwordInput.PasswordBox.Password = newPassword;
        }

        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
    }
}
