using Globe.Client.Platform.Services.Notifications;
using System.Windows;
using System.Windows.Controls;

namespace Globe.Client.Platform.Controls
{
    public class NotificationButton : Button
    {
        #region Constructors

        public NotificationButton()
        {
        }

        static NotificationButton()
        {
            LevelProperty = DependencyProperty.Register("Level", typeof(NotificationLevel), typeof(NotificationButton), new PropertyMetadata(null));
            NotifierProperty = DependencyProperty.Register("Notifier", typeof(NotificationControl), typeof(NotificationButton), new PropertyMetadata(null));
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty LevelProperty;
        public NotificationLevel Level 
        {
            get { return (NotificationLevel)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        public static readonly DependencyProperty NotifierProperty;
        public NotificationControl Notifier
        {
            get { return (NotificationControl)GetValue(NotifierProperty); }
            set { SetValue(NotifierProperty, value); }
        }

        #endregion

        #region Protected Functions

        //protected override void OnClick()
        //{
        //    base.OnClick();

        //    if (this.Notifier != null)
        //        this.Notifier.OnLinkButtonClicked();
        //}

        #endregion
    }
}
