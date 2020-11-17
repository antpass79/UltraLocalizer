using Globe.Client.Platform.Services.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Globe.Client.Platform.Controls
{
    /// <summary>
    /// Interaction logic for NotificationControl.xaml
    /// </summary>
    public partial class NotificationControl : UserControl
    {
        #region Events and Delegates

        private delegate void RefreshDelegate();

        #endregion

        #region Data Members

        Timer _timerLastNotification;

        const int LIMIT = 21;

        #endregion

        #region Constructors

        public NotificationControl()
        {
            InitializeComponent();

            this.seeNotificationHistory.MouseEnter += new MouseEventHandler(SeeNotificationHistoryButton_MouseEnter);
            (this.seeNotificationHistory as Button).Click += new RoutedEventHandler(SeeNotificationHistoryButton_Click);

            this.lastNotification.MouseEnter += new MouseEventHandler(LastNotification_MouseEnter);
            this.lastNotification.MouseLeave += new MouseEventHandler(LastNotification_MouseLeave);

            _timerLastNotification = new Timer(new TimerCallback(CloseLastNotification));

            NotificationCount = 0;
            NotificationExists = false;
        }

        static NotificationControl()
        {
            RemoveAllNotificationsCommandProperty = DependencyProperty.Register("RemoveAllNotificationsCommand", typeof(ICommand), typeof(NotificationControl), new PropertyMetadata(null));
            RemoveNotificationCommandProperty = DependencyProperty.Register("RemoveNotificationCommand", typeof(ICommand), typeof(NotificationControl), new PropertyMetadata(null));
            NewNotificationProperty = DependencyProperty.Register("NewNotification", typeof(Notification), typeof(NotificationControl), new PropertyMetadata(new PropertyChangedCallback(NewNotificationUpdated)));
            NotificationsProperty = DependencyProperty.Register("Notifications", typeof(ObservableCollection<Notification>), typeof(NotificationControl), new PropertyMetadata(new PropertyChangedCallback(NotificationsUpdated)));
            ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(NotificationControl), new PropertyMetadata(null));
            NotificationCountProperty = DependencyProperty.Register("NotificationCount", typeof(int), typeof(NotificationControl), new PropertyMetadata(null));
            NotificationExistsProperty = DependencyProperty.Register("NotificationExists", typeof(bool), typeof(NotificationControl), new PropertyMetadata(false));
        }

        #endregion

        #region Properties

        public bool AreManualNotificationsRemovable
        {
            get { return this.Notifications.Any(item => item.IsManualRemoveable); }
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty RemoveAllNotificationsCommandProperty;
        public ICommand RemoveAllNotificationsCommand
        {
            get { return (ICommand)GetValue(RemoveAllNotificationsCommandProperty); }
            set { SetValue(RemoveAllNotificationsCommandProperty, value); }
        }

        public static readonly DependencyProperty RemoveNotificationCommandProperty;
        public ICommand RemoveNotificationCommand
        {
            get { return (ICommand)GetValue(RemoveNotificationCommandProperty); }
            set { SetValue(RemoveNotificationCommandProperty, value); }
        }

        public static readonly DependencyProperty NewNotificationProperty;
        public Notification NewNotification
        {
            get { return (Notification)GetValue(NewNotificationProperty); }
            set { SetValue(NewNotificationProperty, value); }
        }

        public static readonly DependencyProperty NotificationsProperty;
        public ObservableCollection<Notification> Notifications
        {
            get { return (ObservableCollection<Notification>)GetValue(NotificationsProperty); }
            set { SetValue(NotificationsProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty;
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty NotificationCountProperty;
        public int NotificationCount
        {
            get { return (int)GetValue(NotificationCountProperty); }
            set { SetValue(NotificationCountProperty, value); }
        }

        public static readonly DependencyProperty NotificationExistsProperty;
        public bool NotificationExists
        {
            get { return (bool)GetValue(NotificationExistsProperty); }
            set { SetValue(NotificationExistsProperty, value); }
        }

        #endregion

        #region Private Functions

        void CleanViewedNotifications()
        {
            List<Notification> notifications = this.Notifications.Where(item => !item.IsManualRemoveable).ToList();
            notifications.ForEach(item => this.Notifications.Remove(item));

            if (this.Notifications.Count >= LIMIT)
                this.Notifications.RemoveAt(this.Notifications.Count - 1);
        }

        private void ShowLastNotification(int dueTime)
        {
            this.lastNotification.IsOpen = true;
            this._timerLastNotification.Change(dueTime, System.Threading.Timeout.Infinite);
        }

        void ShowNotifications(bool staysOpen = false)
        {
            this.notificationHistory.IsOpen = true;
            this.lastNotification.IsOpen = false;
        }

        void LastNotification_MouseLeave(object sender, MouseEventArgs e)
        {
            this._timerLastNotification.Change(500, System.Threading.Timeout.Infinite);
        }

        private void LastNotification_MouseEnter(object sender, MouseEventArgs e)
        {
            this._timerLastNotification.Change(40000, System.Threading.Timeout.Infinite);
        }

        void SeeNotificationHistoryButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.NewNotification != null)
                ShowLastNotification(4000);
        }

        void SeeNotificationHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.notificationHistory.IsOpen)
                this.notificationHistory.IsOpen = false;
            else
                ShowNotifications();        
        }

        void Notifications_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotificationCount = Notifications == null ? 0 : Notifications.Count;
            NotificationExists = NotificationCount != 0;

            if (Notifications.Count > 0)
                NewNotification = Notifications[0];
        }

        static void NewNotificationUpdated(DependencyObject instance, DependencyPropertyChangedEventArgs e)
        {
            NotificationControl notificationControl = instance as NotificationControl;

            if (e.NewValue == null)
                return;

            if (!notificationControl.AreManualNotificationsRemovable)
                notificationControl.ShowLastNotification(2000);
            else
                notificationControl.ShowNotifications();
        }

        static void NotificationsUpdated(DependencyObject instance, DependencyPropertyChangedEventArgs e)
        {
            NotificationControl notificationControl = instance as NotificationControl;

            var oldCollection = e.OldValue as ObservableCollection<Notification>;
            var newCollection = e.NewValue as ObservableCollection<Notification>;

            if (oldCollection != null)
                oldCollection.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(notificationControl.Notifications_CollectionChanged);
            if (newCollection != null)
                newCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(notificationControl.Notifications_CollectionChanged);
        }

        private void CloseLastNotification(object param)
        {
            this.lastNotification.Dispatcher.BeginInvoke((Action)(() =>
            {
                this.lastNotification.IsOpen = false;
            }
            ));
        }

        //private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.lastNotification.IsOpen = false;

        //    if (this.notificationHistory.IsOpen)
        //    {
        //        var selectedNotification = this.notificationList.SelectedItem as Notification;
        //        Notifications.Remove(selectedNotification);
        //        NotificationCount = Notifications == null ? 0 : Notifications.Count;
        //        NotificationExists = NotificationCount != 0;
        //    }
        //}

        private void NotificationHistoryPopUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.lastNotification.IsOpen = false;
            this.notificationHistory.IsOpen = false;
        }

        private void DismissAll_Click(object sender, RoutedEventArgs e)
        {
            this.CleanViewedNotifications();
            NotificationCount = 0;
            NotificationExists = false;
        }

        #endregion
    }
}
