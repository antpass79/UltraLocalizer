using Globe.Client.Localizer.Models;
using Globe.Client.Platform.Services.Notifications;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.ComponentModel;

namespace Globe.Client.Localizer.Dialogs.ViewModels
{
    class MessageDialogViewModel : BindableBase, IDialogAware
    {

        public MessageDialogViewModel()
        {

        }

        public string Title
        {
            get { return _notification != null ? _notification.Title : string.Empty; }
        }

        private Notification _notification;
        public Notification Notification
        {
            get { return _notification; }
            private set { SetProperty(ref _notification, value); }
        }

        private DelegateCommand _closeDialogCommand;
        public DelegateCommand CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand(() =>
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
            }));

        public event Action<IDialogResult> RequestClose;
        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            Notification = parameters.GetValue<Notification>("notification");
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

           
        }
    }
}
