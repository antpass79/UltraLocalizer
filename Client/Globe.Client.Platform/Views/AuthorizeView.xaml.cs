using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Globe.Client.Platform.Views
{
    /// <summary>
    /// Interaction logic for AuthorizeWindow.xaml
    /// </summary>
    public partial class AuthorizeView : UserControl
    {
        public AuthorizeView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.Register("PlaceHolder", typeof(object), typeof(AuthorizeView), new PropertyMetadata(null));
        public object PlaceHolder
        {
            get { return (object)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }

        public static readonly DependencyProperty VisibileWithoutRolesProperty =
            DependencyProperty.Register("VisibileWithoutRoles", typeof(bool), typeof(AuthorizeView), new PropertyMetadata(false));
        public bool VisibileWithoutRoles
        {
            get { return (bool)GetValue(VisibileWithoutRolesProperty); }
            set { SetValue(VisibileWithoutRolesProperty, value); }
        }

        public static readonly DependencyProperty UserRolesProperty =
            DependencyProperty.Register("UserRoles", typeof(IEnumerable<string>), typeof(AuthorizeView), new PropertyMetadata(new string[] { }, new PropertyChangedCallback(UserRolesPropertyChanged)));
        public IEnumerable<string> UserRoles
        {
            get { return (IEnumerable<string>)GetValue(UserRolesProperty); }
            set { SetValue(UserRolesProperty, value); }
        }

        public static readonly DependencyProperty RolesProperty =
            DependencyProperty.Register("Roles", typeof(string), typeof(AuthorizeView), new PropertyMetadata(string.Empty, new PropertyChangedCallback(RolesPropertyChanged)));
        public string Roles
        {
            get { return (string)GetValue(RolesProperty); }
            set { SetValue(RolesProperty, value); }
        }

        public static readonly DependencyProperty ContentVisibilityProperty =
            DependencyProperty.Register("ContentVisibility", typeof(Visibility), typeof(AuthorizeView), new PropertyMetadata(Visibility.Collapsed));
        public Visibility ContentVisibility
        {
            get { return (Visibility)GetValue(ContentVisibilityProperty); }
            set { SetValue(ContentVisibilityProperty, value); }
        }

        private static void UserRolesPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var authorizeView = obj as AuthorizeView;
            authorizeView.ChangeContentVisibility();
        }

        private static void RolesPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var authorizeView = obj as AuthorizeView;
            authorizeView.ChangeContentVisibility();
        }

        private void ChangeContentVisibility()
        {
            if (VisibileWithoutRoles)
            {
                this.ContentVisibility = Visibility.Visible;
                return;
            }

            if (this.UserRoles == null)
                return;

            bool contentVisible = false;
            string[] roles = this.Roles.Split(',', System.StringSplitOptions.RemoveEmptyEntries);

            foreach(var role in roles)
            {
                contentVisible = this.UserRoles.Contains(role.Trim());
                if (contentVisible)
                    break;
            }

            this.ContentVisibility = contentVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
